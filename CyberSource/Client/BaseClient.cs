using CyberSource.Base;
using System;
using System.Collections;
using System.Net;
using System.ServiceModel;
using System.Xml.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;
using System.Security;
using System.Reflection;
using System.Linq;

namespace CyberSource.Clients
{
    /// <summary>
    /// Base class for all clients.
    /// </summary>
    public abstract class BaseClient
    {
        /// <summary>
        /// Version of this client.
        /// </summary>
        public static string CLIENT_LIBRARY_VERSION;
        public const string CYBS_SUBJECT_NAME = "CyberSource_SJC_US";

        /// <summary>
        /// Proxy object that is initialized during start-up, if needed.
        /// </summary>
        protected static WebProxy mProxy = null;

        /// <summary>
        /// String containing information about the working environment.
        /// </summary>
        protected static string mEnvironmentInfo =
                  Environment.OSVersion.Platform +
                  Environment.OSVersion.Version.ToString() + "-CLR" +
                  Environment.Version.ToString();

        /// <summary>
        /// Field for merchantID used in requests.
        /// </summary>
        protected const string MERCHANT_ID = "merchantID";

        private const string PROXY_URL = "proxyURL";
        private const string PROXY_USER = "proxyUser";
        private const string PROXY_PASSWORD = "proxyPassword";
        private const string BASIC_AUTH = "Basic";

        public const string CYBERSOURCE_PUBLIC_KEY = "CyberSource_SJC_US";
        public const string X509_CLAIMTYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname";
        protected static ConcurrentDictionary<string, CertificateEntry> merchantIdentities = new ConcurrentDictionary<string, CertificateEntry>();

        static BaseClient()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | (SecurityProtocolType)768;
            CLIENT_LIBRARY_VERSION = GetClientId();
            SetupProxy();
        }

        private static string GetClientId()
        {
            var assembly = typeof(Configuration).Assembly;

            var assemblyVersion = AssemblyHelper.GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly).Version;

            return assemblyVersion;
        }

        private static void SetupProxy()
        {
            // if a proxyURL is specified in the configuration file,
            // create a WebProxy object for later use.
            string proxyURL = AppSettings.GetSetting(null, PROXY_URL);
            if (proxyURL != null)
            {
                mProxy = new WebProxy(proxyURL);

                // if a proxyUser is specified in the configuration file,
                // set up the proxy object's Credentials.
                string proxyUser
                    = AppSettings.GetSetting(null, PROXY_USER);
                if (proxyUser != null)
                {
                    SecureString proxyPassword = new SecureString();

                    foreach (char c in AppSettings.GetSetting(null, PROXY_PASSWORD))
                    {
                        proxyPassword.AppendChar(c);
                    }

                    proxyPassword.MakeReadOnly();

                    NetworkCredential credential
                        = new NetworkCredential(proxyUser, proxyPassword);

                    CredentialCache cache = new CredentialCache();
                    cache.Add(new Uri(proxyURL), BASIC_AUTH, credential);
                    mProxy.Credentials = cache;
                    proxyPassword.Dispose();
                }
            }
        }

        /// <summary>
        /// Builds a Configuration object using settings from the
        /// appsettings section of the application/web config file.
        /// This is a public static method that can potentially be
        /// used to create and cache Configuration object(s) when
        /// the application starts.
        /// </summary>
        /// <param name="merchantID">
        /// If not null, this method will get the settings specific to
        /// this merchant id.
        /// </param>
        /// <returns>the built Configuration object</returns>
        public static Configuration BuildConfiguration(
            string merchantID)
        {
            return (InternalBuildConfiguration(merchantID, false));
        }

        /// <summary>
        /// Builds a Configuration object using settings from the
        /// appsettings section of the application/web config file.
        /// </summary>
        /// <param name="requestMerchantID">
        /// merchantID in the request.  If not null, this method will get
        /// the settings specific to this merchant id.
        /// </param>
        /// <returns>the built Configuration object</returns>
        protected static Configuration BuildConfigurationForRequest(
            string requestMerchantID)
        {
            return (InternalBuildConfiguration(requestMerchantID, true));
        }

        /// <summary>
        /// Builds a Configuration object using settings from the
        /// appsettings section of the application/web config file.
        /// </summary>
        /// <param name="merchantID">
        /// If not null, this method will get the settings specific to
        /// this merchant id.
        /// </param>
        /// <param name="failIfNoMerchantID">
        /// If set to true, an ApplicationException will be thrown if
        /// merchantID is null and there is no merchantID in the
        /// appsettings, either.
        /// </param>
        /// <returns>the built Configuration object</returns>
        private static Configuration InternalBuildConfiguration(
            string merchantID, bool failIfNoMerchantID)
        {
            Configuration config = new Configuration();

            if (merchantID == null)
            {
                merchantID
                    = AppSettings.GetSetting(null, MERCHANT_ID);
            }
            if (merchantID != null || failIfNoMerchantID)
            {
                // if merchantID is null, the assignment below will
                // throw an exception.
                config.MerchantID = merchantID;
            }

            string keysDirectory
                = AppSettings.GetSetting(
                    merchantID, Configuration.KEYS_DIRECTORY);
            if (keysDirectory != null)
            {
                config.KeysDirectory = keysDirectory;
            }
            else
            {
                // look for "keysDir" for backwards-compatibility
                config.KeysDirectory
                    = AppSettings.GetSetting(
                        merchantID, Configuration.KEYS_DIR);
            }

            int boolVal
                = AppSettings.GetBoolSetting(
                    merchantID, Configuration.SEND_TO_PRODUCTION);
            if (boolVal != -1) config.SendToProduction = (boolVal == 1);

            boolVal
                = AppSettings.GetBoolSetting(
                    merchantID, Configuration.ENABLE_LOG);
            config.setLogProperties(
                boolVal == 1,
                AppSettings.GetSetting(
                    merchantID, Configuration.LOG_DIRECTORY));

            config.ServerURL
                = AppSettings.GetSetting(
                    merchantID, Configuration.SERVER_URL);
            if (config.ServerURL == null)
            {
                // look for "cybersourceURL" for backwards-compatibility
                config.ServerURL
                    = AppSettings.GetSetting(
                        merchantID, Configuration.CYBERSOURCE_URL);
            }

            //See if akamai flag has been set or not which eventually gives effective server URL
            boolVal
                = AppSettings.GetBoolSetting(
                    merchantID, Configuration.SEND_TO_AKAMAI);
            if (boolVal != -1) config.SendToAkamai = (boolVal == 1);

            config.KeyFilename
                = AppSettings.GetSetting(
                    merchantID, Configuration.KEY_FILENAME);

            config.KeyAlias
               = AppSettings.GetSetting(
                   merchantID, Configuration.KEY_ALIAS);

            config.Password
                = convertToSecureString(AppSettings.GetSetting(
                    merchantID, Configuration.PASSWORD));

            config.LogFilename
                = AppSettings.GetSetting(
                    merchantID, Configuration.LOG_FILENAME);

            config.LogMaximumSize
                = AppSettings.GetIntSetting(
                    merchantID, Configuration.LOG_MAXIMUM_SIZE);

            config.Timeout
                = AppSettings.GetIntSetting(
                    merchantID, Configuration.TIMEOUT);

            boolVal
                = AppSettings.GetBoolSetting(
                    merchantID, Configuration.DEMO);
            if (boolVal != -1) config.Demo = (boolVal == 1);

            config.ConnectionLimit
                = AppSettings.GetIntSetting(
                    merchantID, Configuration.CONNECTION_LIMIT);

            // Encryption enable flag
            boolVal
               = AppSettings.GetBoolSetting(
                   merchantID, Configuration.USE_SIGNED_AND_ENCRYPTED);
            if (boolVal != -1) config.UseSignedAndEncrypted = (boolVal == 1);

            // certificate cache flag
            boolVal
               = AppSettings.GetBoolSetting(
                   merchantID, Configuration.CERTIFICATE_CACHE_ENABLED);
            if (boolVal != -1) config.CertificateCacheEnabled = (boolVal == 1);

            return (config);
        }

        /// <summary>
        /// Prepares the log file and logs transaction-start stuff
        /// if logging is enabled.  Otherwise, it does nothing.
        /// </summary>
        /// <param name="config">
        /// Configuration object for the current transaction.</param>
        /// <returns>
        /// the Logger object if logging is enabled.  Returns null
        /// otherwise.
        /// </returns>
        protected static Logger PrepareLog(Configuration config)
        {
            Logger logger = null;
            if (config.EnableLog)
            {
                logger
                    = Logger.GetInstance(
                        config.LogDirectory, config.LogFilename,
                        config.LogMaximumSize);
                if (logger != null)
                {
                    logger.LogTransactionStart(config.LogString);
                }
            }
            return (logger);
        }

        /// <summary>
        /// This method will extract the XmlElementAttribute from the
        /// custom attributes and get its Namespace property.
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// the Namespace property of the XmlElementAttribute.
        /// </returns>
        protected static string GetXmlElementAttributeNamespace(Type type)
        {
            //With WCF the namespace is no longer used. But it might be nice to have it.
            //However I don't want the client to fail if for some reason it can't get the namespace;

            Logger logger = null;
            try
            {
                logger = PrepareLog(InternalBuildConfiguration(null, false));

                switch (type.Name)
                {
                    case "RequestMessage":
                        return ((XmlTypeAttribute)type.GetCustomAttributes(typeof(XmlTypeAttribute), false)[0]).Namespace;
                    case "inputNVPMessageIn":
                        return ((MessageBodyMemberAttribute)type.GetMember("nvpRequest")[0].GetCustomAttributes(typeof(MessageBodyMemberAttribute), false)[0]).Namespace;
                    default:
                        return "";
                }
            }
            catch
            {
                if (logger != null)
                {
                    logger.Log(Logger.LogType.CONFIG, "Failed to get Namespace from Service Reference. This should not prevent the client from working: Type=" + type.FullName);
                }
                return "";
            }
        }

        /// <summary>
        /// Sets the ConnectionLimit of the ServicePoint object for the
        /// URL in the Configuration object.
        /// </summary>
        /// <param name="config">
        /// Configuration object containing the URL and the connection
        /// limit.
        /// </param>
        protected static void SetConnectionLimit(Configuration config)
        {
            if (config.ConnectionLimit != -1)
            {
                Uri uri = new Uri(config.EffectiveServerURL);
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLimit = config.ConnectionLimit;
            }
        }


        /// <summary>
        /// Returns a custom wcf binding that will create a SOAP request 
        /// compatible with the Simple Order API Service
        /// </summary>
        protected static CustomBinding getWCFCustomBinding(Configuration config)
        {
            //Setup custom binding with HTTPS + Body Signing 
            CustomBinding currentBinding = new CustomBinding();

            //Sign the body
            AsymmetricSecurityBindingElement asec = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);
            asec.SetKeyDerivation(false);
            asec.IncludeTimestamp = false;
            asec.EnableUnsecuredResponse = true;
            asec.SecurityHeaderLayout = SecurityHeaderLayout.Lax;

            if (config.UseSignedAndEncrypted)
            {
                asec.LocalClientSettings.IdentityVerifier = new CustomeIdentityVerifier();
                asec.RecipientTokenParameters = new System.ServiceModel.Security.Tokens.X509SecurityTokenParameters { InclusionMode = SecurityTokenInclusionMode.Once };
                asec.MessageProtectionOrder = System.ServiceModel.Security.MessageProtectionOrder.SignBeforeEncrypt;
                asec.EndpointSupportingTokenParameters.SignedEncrypted.Add(new System.ServiceModel.Security.Tokens.X509SecurityTokenParameters());
                asec.SetKeyDerivation(false);
            }

            //Use custom encoder to strip unsigned timestamp in response
            CustomTextMessageBindingElement textBindingElement = new CustomTextMessageBindingElement();


            //Setup https transport 
            HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
            httpsTransport.RequireClientCertificate = true;
            httpsTransport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            httpsTransport.MaxReceivedMessageSize = 2147483647;
            httpsTransport.UseDefaultWebProxy = false;

            //Setup Proxy if needed
            if (mProxy != null)
            {
                WebRequest.DefaultWebProxy = mProxy;
                httpsTransport.UseDefaultWebProxy = true;
            }


            //Bind in order (Security layer, message layer, transport layer)
            currentBinding.Elements.Add(asec);
            currentBinding.Elements.Add(textBindingElement);
            currentBinding.Elements.Add(httpsTransport);
            return currentBinding;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="merchantIdentities"></param>
        /// <returns></returns>
        protected static X509Certificate2 GetOrFindValidMerchantCertFromStore(string merchantId, ConcurrentDictionary<string, CertificateEntry> merchantIdentities)
        {
            return merchantIdentities[merchantId] != null ? merchantIdentities[merchantId].MerchantCert : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="merchantIdentities"></param>
        /// <returns></returns>
        protected static X509Certificate2 GetOrFindValidCybsCertFromStore(string merchantId, ConcurrentDictionary<string, CertificateEntry> merchantIdentities)
        {
            return merchantIdentities[merchantId] != null ? merchantIdentities[merchantId].CybsCert : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantIdentities"></param>
        /// <param name="logger"></param>
        /// <param name="merchantId"></param>
        /// <param name="creationTime"></param>
        /// <returns></returns>
        public static bool IsMerchantCertExpired(Logger logger, string merchantId, DateTime modifiedTime, ConcurrentDictionary<string, CertificateEntry> merchantIdentities)
        {
            if (merchantIdentities[merchantId] != null)
            {
                if (merchantIdentities[merchantId].ModifiedTime != modifiedTime)
                {
                    if (logger != null)
                    {
                        logger.LogInfo("certificate is expired, will be loaded again in memory for merchantID: " + merchantId);
                    }
                    return true;
                }

            }
            return false;
        }

        private static SecureString convertToSecureString(string originalString)
        {
            if (originalString == null)
            {
                return null;
            }

            var secureString = new SecureString();

            foreach (char c in originalString)
                secureString.AppendChar(c);

            secureString.MakeReadOnly();
            return secureString;
        }
    }

    /// <summary>
    /// Code to process AssemblyInfo.cs information
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ass"></param>
        /// <returns></returns>
        public static T GetAssemblyAttribute<T>(this Assembly ass) where T : Attribute
        {
            object[] attributes = ass.GetCustomAttributes(typeof(T), false);
            if (attributes == null || attributes.Length == 0)
                return null;
            return attributes.OfType<T>().SingleOrDefault();
        }
    }
}
