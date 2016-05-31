using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using CyberSource.Base;
using CyberSource.Clients.SoapServiceReference;
using System.Xml;


namespace CyberSource.Clients
{
	/// <summary>
	/// CyberSource Web Services Soap Client class.
	/// </summary>
	public class SoapClient : BaseClient
	{
        /// <summary>
        /// Namespace URI used for CyberSource-specific elements.
        /// </summary>
        public static readonly string CYBS_NAMESPACE;

        static SoapClient()
        {
            CYBS_NAMESPACE = GetXmlElementAttributeNamespace(typeof(RequestMessage));
        }


		private SoapClient() {}

        /// <summary>
        /// Sends a CyberSource transaction request.
        /// </summary>
		/// <param name="requestMessage">RequestMessage object containing the request.</param>
		/// <returns>ReplyMessage containing the reply.</returns>
        public static ReplyMessage RunTransaction(
            RequestMessage requestMessage )
        {
            return (RunTransaction(null, requestMessage));
        }

        /// <summary>
        /// Sends a CyberSource transaction request.
        /// </summary>
        /// <param name="config">Configuration object to use.</param>
		/// <param name="requestMessage">RequestMessage object containing the request.</param>
		/// <returns>ReplyMessage containing the reply.</returns>
        public static ReplyMessage RunTransaction(
            Configuration config, RequestMessage requestMessage)
        {

            Logger logger = null;
            TransactionProcessorClient proc = null;
			try
			{

                DetermineEffectiveMerchantID(ref config, requestMessage);
                SetVersionInformation(requestMessage);
                logger = PrepareLog(config);
                SetConnectionLimit(config);


                CustomBinding currentBinding = getWCFCustomBinding();


                //Setup endpoint Address with dns identity
                AddressHeaderCollection headers = new AddressHeaderCollection();
                EndpointAddress endpointAddress = new EndpointAddress( new Uri(config.EffectiveServerURL), EndpointIdentity.CreateDnsIdentity(config.EffectivePassword), headers );
                
                //Get instance of service
                using( proc = new TransactionProcessorClient(currentBinding, endpointAddress)){
                
                    //Set protection level to sign only
                    proc.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;

                    // set the timeout
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, config.Timeout, 0);
                    currentBinding.SendTimeout = timeOut;
              
                    //add certificate credentials
                    string keyFilePath = Path.Combine(config.KeysDirectory,config.EffectiveKeyFilename);
                    proc.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(keyFilePath,config.EffectivePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                    proc.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;

                    // Changes for SHA2 certificates support
                    X509Certificate2Collection collection = new X509Certificate2Collection();
                    collection.Import(keyFilePath, config.EffectivePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                    foreach (X509Certificate2 cert1 in collection)
                    {
                        if (cert1.Subject.Contains(config.MerchantID))
                        {
                            proc.ClientCredentials.ClientCertificate.Certificate = cert1;
                            proc.ClientCredentials.ServiceCertificate.DefaultCertificate = cert1;
                            break;
                        }
                    }

                    // send request now
                    // Changes for NGT-3035
                    XmlNode req = SerializeObjectToXmlNode(requestMessage);
                    if (logger != null)
                    {
                        logger.LogRequest(req, config.Demo);
                    }                   
                    
                    ReplyMessage reply = proc.runTransaction(requestMessage);
                    XmlNode rep = SerializeObjectToXmlNode(reply);
                    if (logger != null)
                    {
                        logger.LogReply(rep, config.Demo);
                    }  
                   
                    return (reply);
                }
			}
		    catch (Exception e)
            {
                if (logger != null)
                {
                    logger.LogException(e);
                }
                if (proc != null)
                {
                    proc.Abort();
                }
                throw;
            }
            finally
            {
                if (proc != null)
                {
                    proc.Close();
                }
            }
        }

        // Changes for NGT-3035
        private static XmlNode SerializeObjectToXmlNode(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Argument cannot be null");

            XmlNode resultNode = null;
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("", "");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    xmlSerializer.Serialize(memoryStream, obj, ns);
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
                memoryStream.Position = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load(memoryStream);
                resultNode = doc.DocumentElement;
            }

            return resultNode;
        }

     
        private static void DetermineEffectiveMerchantID(
            ref Configuration config, RequestMessage request)
        {
            string requestMerchantID = request.merchantID;

            if (config == null)
            {
                // let's build a config object on the fly using
                // the merchantID from the request.  An exception will
                // be thrown if requestMerchantID is null and 
                // no merchantID is found in the config file.
                config = BuildConfigurationForRequest(requestMerchantID);
            }

            if (requestMerchantID == null)
            {
                // No merchantID in the request; get it from the config.
                // NonNullMerchantID will throw an exception if
                // MerchantID is null.
                request.merchantID = config.NonNullMerchantID;
            }
            // else, there is a merchantID in the request.
            // we do not have to do anything.  We'll keep whatever
            // merchantID is in the Configuration object as we do
            // not own that object.
        }

        private static void SetVersionInformation(
			RequestMessage requestMessage )
		{
			requestMessage.clientLibrary = ".NET Soap";
			requestMessage.clientLibraryVersion = CLIENT_LIBRARY_VERSION;
			requestMessage.clientEnvironment = mEnvironmentInfo;
			requestMessage.clientSecurityLibraryVersion =".Net 1.0.0";
		}
	}
}
