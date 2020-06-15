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


                CustomBinding currentBinding = getWCFCustomBinding(config);


                //Setup endpoint Address with dns identity
                AddressHeaderCollection headers = new AddressHeaderCollection();
                EndpointAddress endpointAddress = new EndpointAddress( new Uri(config.EffectiveServerURL), EndpointIdentity.CreateDnsIdentity(config.EffectivePassword), headers );
                
                //Get instance of service
                using( proc = new TransactionProcessorClient(currentBinding, endpointAddress))
                {
                    // set the timeout
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, config.Timeout, 0);
                    currentBinding.SendTimeout = timeOut;
              
                    //add certificate credentials
                    string keyFilePath = Path.Combine(config.KeysDirectory,config.EffectiveKeyFilename);

                    X509Certificate2 merchantCert = null;
                    X509Certificate2 cybsCert = null;
                    DateTime dateFile = File.GetCreationTime(keyFilePath);
                    if (config.CertificateCacheEnabled) 
                    {
                        if (!merchantIdentities.ContainsKey(config.MerchantID) || IsMerchantCertExpired(logger, config.MerchantID, dateFile.ToFileTimeUtc(), merchantIdentities))
                        {
                            if (logger != null)
                            {
                                logger.LogInfo("Loading certificate for merchantID " + config.MerchantID);
                            }

                            X509Certificate2Collection collection = new X509Certificate2Collection();
                            collection.Import(keyFilePath, config.EffectivePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                            X509Certificate2 newMerchantCert = null;
                            X509Certificate2 newCybsCert = null;

                            foreach (X509Certificate2 cert1 in collection)
                            {
                                if (cert1.Subject.Contains(config.MerchantID))
                                {
                                    newMerchantCert = cert1;
                                }

                                if (cert1.Subject.Contains(CYBS_SUBJECT_NAME))
                                {
                                    newCybsCert = cert1;
                                }
                            }
                            if (merchantIdentities.ContainsKey(config.MerchantID))
                            {
                                merchantIdentities.Remove(config.MerchantID);
                            }
                            merchantIdentities.Add(config.MerchantID, new CertificateEntry(dateFile.ToFileTimeUtc(), newMerchantCert, newCybsCert));

                        }
                        merchantCert = GetOrFindValidMerchantCertFromStore(config.MerchantID, merchantIdentities);
                        if (config.UseSignedAndEncrypted)
                        {
                            cybsCert = GetOrFindValidCybsCertFromStore(config.MerchantID, merchantIdentities);
                        }
                    }
                    else
                    {
                        // Changes for SHA2 certificates support
                        X509Certificate2Collection collection = new X509Certificate2Collection();
                        collection.Import(keyFilePath, config.EffectivePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                        foreach (X509Certificate2 cert1 in collection)
                        {
                            if (cert1.Subject.Contains(config.MerchantID))
                            {
                                merchantCert = cert1;
                                break;
                            }
                        }

                        if (config.UseSignedAndEncrypted)
                        {
                            foreach (X509Certificate2 cert2 in collection)
                            {
                                //Console.WriteLine(cert1.Subject);
                                if (cert2.Subject.Contains(CYBERSOURCE_PUBLIC_KEY))
                                {
                                    cybsCert = cert2;
                                    break;
                                }
                            }
                        }
                    }

                    if (merchantCert == null)
                    {
                        throw new ApplicationException(
                    "CONFIGURATION OR CODE BUG:  merchant certificate is missing, check the p12 file");
                    }
                    //Set protection level to sign only
                    proc.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                    proc.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                    proc.ClientCredentials.ClientCertificate.Certificate = merchantCert;
                    proc.ClientCredentials.ServiceCertificate.DefaultCertificate = merchantCert;

                    if (config.UseSignedAndEncrypted)
                    {
                        if (cybsCert == null)
                        {
                            throw new ApplicationException(
                        "CONFIGURATION OR CODE BUG:  cybs certificate is missing, check the p12 file");
                        }

                        //Set protection level to sign & encrypt only
                        proc.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                        proc.ClientCredentials.ServiceCertificate.DefaultCertificate = cybsCert;
                    }
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

                // Fix for Xml external entity injection violation in fortify report
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Prohibit;
                settings.XmlResolver = null;
                XmlReader reader = XmlReader.Create(memoryStream, settings);
                doc.Load(reader);

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
			requestMessage.clientSecurityLibraryVersion =".Net 1.4.3";
		}
	}
}
