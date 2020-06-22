using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using CyberSource.Base;
using CyberSource.Clients.NVPServiceReference;



namespace CyberSource.Clients
{
    /// <summary>
    /// CyberSource Web Services Basic Client class.
    /// </summary>
    public class NVPClient : BaseClient
    {
        /// <summary>
        /// Namespace URI used for CyberSource-specific elements.
        /// </summary>
        public static readonly string CYBS_NAMESPACE;

        static NVPClient()
        {
            
            CYBS_NAMESPACE = GetXmlElementAttributeNamespace(typeof(inputNVPMessageIn));
           
        }

        private NVPClient() { }

        /// <summary>
        /// Sends a CyberSource transaction request.
        /// </summary>
        /// <param name="request">Hashtable containing the request fields and their values.</param>
        /// <returns>Hashtable containing the reply fields and their values.</returns>
        public static Hashtable RunTransaction(Hashtable request)
        {
            return (RunTransaction(null, request));
        }

        /// <summary>
        /// Sends a CyberSource transaction request.
        /// </summary>
        /// <param name="config">Configuration object to use.</param>
        /// <param name="request">Hashtable containing the request fields and their values.</param>
        /// <returns>Hashtable containing the reply fields and their values.</returns>
        public static Hashtable RunTransaction(
            Configuration config, Hashtable request)
        {
            Logger logger=null;
            NVPTransactionProcessorClient proc=null;
            try
            {
                DetermineEffectiveMerchantID(ref config, request);
                SetVersionInformation(request);
                logger = PrepareLog(config);
                SetConnectionLimit(config);

                //Setup custom binding with HTTPS + Body Signing 
                CustomBinding currentBinding = getWCFCustomBinding(config);

                //Setup endpoint Address with dns identity
                AddressHeaderCollection headers = new AddressHeaderCollection();
                EndpointAddress endpointAddress = new EndpointAddress(new Uri(config.EffectiveServerURL), EndpointIdentity.CreateDnsIdentity(config.EffectivePassword), headers);

                //Get instance of service
                using (proc = new NVPTransactionProcessorClient(currentBinding, endpointAddress))
                {
                    // set the timeout
                    TimeSpan timeOut = new TimeSpan(0, 0, 0, config.Timeout, 0);
                    currentBinding.SendTimeout = timeOut;


                    string keyFilePath = Path.Combine(config.KeysDirectory, config.EffectiveKeyFilename);
                    X509Certificate2 merchantCert = null;
                    X509Certificate2 cybsCert = null;
                    DateTime dateFile = File.GetCreationTime(keyFilePath);
                    if (config.CertificateCacheEnabled)
                    {
                        if (!merchantIdentities.ContainsKey(config.MerchantID) || IsMerchantCertExpired(logger, config.MerchantID, dateFile, merchantIdentities))
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
                            CertificateEntry newCert = new CertificateEntry
                            {
                                CreationTime = dateFile,
                                CybsCert = newCybsCert,
                                MerchantCert = newMerchantCert
                            };
                            merchantIdentities.AddOrUpdate(config.MerchantID, newCert, (x, y) => newCert);
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
                    //Set protection level to sign
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

                    if (logger != null)
                    {
                        logger.LogRequest(request, config.Demo);
                    }

                    // send request now, converting the hashtable request into
                    // a string, and the string reply back into a hashtable.

                    string resp = proc.runTransaction(Hash2String(request));


                    Hashtable reply = String2Hash(resp);

                    if (logger != null)
                    {
                        logger.LogReply(reply, config.Demo);
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

        private static void DetermineEffectiveMerchantID(
            ref Configuration config, Hashtable request)
        {
            string requestMerchantID = (string)request[MERCHANT_ID];

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
                request[MERCHANT_ID] = config.NonNullMerchantID;
            }
            // else, there is a merchantID in the request.
            // we do not have to do anything.  We'll keep whatever
            // merchantID is in the Configuration object as we do
            // not own that object.
        }

        // will return an empty string if the hashtable is empty.
        private static String Hash2String(Hashtable src)
        {
            StringBuilder dest = new StringBuilder();
            foreach (string key in src.Keys)
            {
                dest.AppendFormat("{0}={1}\n", key, src[key]);
            }

            return (dest.ToString());
        }

        // will return an empty hashtable if the string is empty.
        private static Hashtable String2Hash(string src)
        {
            char[] EQUAL_SIGN = { '=' };

            Hashtable dest = new Hashtable();
            StringReader reader = new StringReader(src);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(EQUAL_SIGN, 2);
                if (parts.Length > 0)
                {
                    dest.Add(
                        parts[0],
                        parts.Length == 2 ? parts[1] : null);
                }
            }

            return (dest);
        }

        private static void SetVersionInformation(Hashtable request)
        {
            request["clientLibrary"] = ".NET NVP";
            request["clientLibraryVersion"] = CLIENT_LIBRARY_VERSION;
            request["clientEnvironment"] = mEnvironmentInfo;
            request["clientSecurityLibraryVersion"] =".Net 1.4.3";
        }
    }
}
