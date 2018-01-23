using System;
using System.IO;

namespace CyberSource.Clients
{
    /// <summary>
    /// Configuration class.  When a Configuration object is passed
    /// to a client's RunTransaction() method, the client will use
    /// its properties instead of picking them up from the application/
    /// web config file.
    /// </summary>
    public class Configuration
    {
        internal const string MERCHANT_ID = "merchantID";
        internal const string KEYS_DIRECTORY = "keysDirectory";
        internal const string KEYS_DIR = "keysDir";
        internal const string SEND_TO_PRODUCTION = "sendToProduction";
        internal const string ENABLE_LOG = "enableLog";
        internal const string LOG_DIRECTORY = "logDirectory";
        internal const string SERVER_URL = "serverURL";
        internal const string CYBERSOURCE_URL = "cybersourceURL";
        internal const string KEY_FILENAME = "keyFilename";
        internal const string PASSWORD = "password";
        internal const string LOG_FILENAME = "logFilename";
        internal const string LOG_MAXIMUM_SIZE = "logMaximumSize";
        internal const string TIMEOUT = "timeout";
        internal const string DEMO = "demo";
        internal const string CONNECTION_LIMIT = "connectionLimit";
        internal const string SEND_TO_AKAMAI = "sendToAkamai";
        internal const string EFFECTIVE_SERVER_URL = "effectiveServerURL";
        internal const string USE_SIGNED_AND_ENCRYPTED = "useSignAndEncrypted";

        /// <summary>
        /// Default log file name.
        /// </summary>
        public const string DEFAULT_LOG_FILENAME = "cybs.log";

        /// <summary>
        /// Default maximum size (in MB) for the log file.
        /// </summary>
        public const int DEFAULT_LOG_MAXIMUM_SIZE = 10;

        /// <summary>
        /// Default timeout (in seconds).
        /// </summary>
        public const int DEFAULT_TIMEOUT = 130;

        private const string TEST_URL = "https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor";
        private const string PROD_URL = "https://ics2ws.ic3.com/commerce/1.x/transactionProcessor";
        private const string AKAMAI_TEST_URL = "https://ics2wstesta.ic3.com/commerce/1.x/transactionProcessor";
        private const string AKAMAI_PROD_URL = "https://ics2wsa.ic3.com/commerce/1.x/transactionProcessor";
        private const string PROD_HOST = "ics2ws.ic3.com";
        
        private const string P12_EXTENSION = ".p12";

        private const string NV_SEPARATOR = "=";
        private const string NVP_SEPARATOR = ",";

        private string merchantID = null;
        private string keysDirectory = null;
        private bool sendToProduction = false;
        private bool enableLog = false;
        private string logDirectory = null;
        private string serverURL = null;
        private string keyFilename = null;
        private byte[] key = null;
        private string password = null;
        private string logFilename = null;
        private int logMaximumSize = -1;
        private int timeout = -1;
        private bool demo = false;
        private bool sendToAkamai = true;
        private int connectionLimit = -1;
        private bool useSignedAndEncrypted = false;

        private bool isSendToProductionSet = false;

        /// <summary>
        /// Corresponds to [cybs.]merchantID in the config file.  The
        /// merchantID in the request, if any, overrides the value in
        /// this property.
        /// </summary>
        public string MerchantID
        {
            get { return merchantID; }
            set
            {
                merchantID = value;
                CheckMerchantID();
            }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].keysDirectory in the config file.
        /// This property must be set.
        /// </summary>
        public string KeysDirectory
        {
            get { return keysDirectory; }
            set {
                keysDirectory = value;
                if (keysDirectory == null)
                {
                    throw new ApplicationException(
                        "CONFIGURATION BUG:  keysDirectory is missing!");
                }
            }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].sendToProduction in the
        /// config file.  Either ServerURL or SendToProduction must be
        /// set.  If both are set, the value of ServerURL takes precedence.
        /// </summary>
        public bool SendToProduction
        {
            get { return sendToProduction; }
            set { sendToProduction = value; isSendToProductionSet = true; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].sendToAkamai 
        /// This flag decides if the transaction will go via akamai or not.
        /// </summary>
        public bool SendToAkamai
        {
            get { return sendToAkamai; }
            set { sendToAkamai = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].enableLog in the config file.
        /// This is a read-only property.  You must call SetLogProperties()
        /// to set both EnableLog and LogDirectory.
        /// </summary>
        public bool EnableLog
        {
            get { return enableLog; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].logDirectory in the config
        /// file.  Must point to an existing directory when EnableLog is
        /// true.
        /// 
        /// This is a read-only property.  You must call SetLogProperties()
        /// to set both EnableLog and LogDirectory.
        /// </summary>
        public string LogDirectory
        {
            get { return logDirectory; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].serverURL or
        /// [cybs.][merchantID].cybersourceURL in the config file.
        /// 
        /// This is optional.  When set, it overrides SendToProduction.
        /// </summary>
        public string ServerURL
        {
            get { return serverURL; }
            set { serverURL = value; }
        }

        /// <summary>
        /// This is optional. When set, it reads key from memory rather than from file system
        /// </summary>
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].keyFilename.
        /// 
        /// This is optional.  When not set, the key filename is derived
        /// from the value of MerchantID.
        /// </summary>
        public string KeyFilename
        {
            get { return keyFilename; }
            set { keyFilename = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].password
        /// 
        /// This is optional.  When not set, the value of MerchantID is
        /// used.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].logFilename.
        /// 
        /// This is optional.  When not set, it defaults to the value of
        /// the constant DEFAULT_LOG_FILENAME.
        /// </summary>
        public string LogFilename
        {
            get
            {
                return(
                    logFilename != null
                        ? logFilename
                        : DEFAULT_LOG_FILENAME );
            }
            set { logFilename = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].logMaximumSize.
        /// 
        /// It is expressed in MB, i.e. a value of 1 indicates 1 MB.
        /// 
        /// It is optional.  When not set, it defaults to the value of
        /// the constant DEFAULT_LOG_MAXIMUM_SIZE.
        /// </summary>
        public int LogMaximumSize
        {
            get
            {
                return (
                    logMaximumSize != -1
                        ? logMaximumSize
                        : DEFAULT_LOG_MAXIMUM_SIZE );
            }
            set { logMaximumSize = value; }
        }

        /// <summary>
        /// Sets the log properties EnableLog and LogDirectory.  We need
        /// them to be set together as we need to make sure the latter is
        /// set if the former is set to true.
        /// </summary>
        /// <param name="enableLog">
        /// whether or not logging is to be enabled.</param>
        /// <param name="logDirectory">
        /// log directory.  Must be non-null if enableLog is true.
        /// </param>
        public void setLogProperties(bool enableLog, string logDirectory)
        {
            if (enableLog && logDirectory == null)
            {
                throw new ApplicationException (
                        "CONFIGURATION BUG:  logDirectory is required when enableLog is true!");
            }
            this.enableLog = enableLog;
            this.logDirectory = logDirectory;
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].timeout.
        /// 
        /// It is expressed in seconds.
        /// 
        /// It is optional.  When not set, it defaults to the value of
        /// the constant DEFAULT_TIMEOUT.
        /// </summary>
        public int Timeout
        {
            get
            {
                return (
                    timeout != -1
                        ? timeout
                        : DEFAULT_TIMEOUT);
            }
            set { timeout = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].demo in the config file.
        /// When true, the request and reply is dumped to the console
        /// (i.e. via Console.WriteLine).  Please note that this property
        /// has no effect if sendToProduction is true or serverURL
        /// is pointing to ics2ws.ic3.com.
        /// </summary>
        public bool Demo
        {
            get
            {
                return( demo &&
                        !sendToProduction &&
                        (serverURL == null ||
                         !serverURL.Contains( PROD_HOST )) );
            }

            set { demo = value; }
        }

        /// <summary>
        /// Corresponds to [cybs.][merchantID].connectionLimit in the
        /// config file.  See README for details.
        /// </summary>
        public int ConnectionLimit
        {
            get { return (connectionLimit); }
            set { connectionLimit = value; }
        }

        /// <summary>
        /// Returns the string representation of this object's current
        /// state suitable for logging.
        /// </summary>
        public string LogString
        {
            get
            {
                string buf = String.Empty;

                if (keysDirectory != null)
                {
                    buf += KEYS_DIRECTORY + NV_SEPARATOR + keysDirectory;
                }

                if (serverURL != null)
                {
                    buf += NVP_SEPARATOR + SERVER_URL + NV_SEPARATOR + serverURL;
                }
                else
                {
                    buf += NVP_SEPARATOR + SEND_TO_PRODUCTION + NV_SEPARATOR + sendToProduction;
                }

                if (keyFilename != null)
                {
                    buf += NVP_SEPARATOR + KEY_FILENAME + NV_SEPARATOR + keyFilename;
                }

                if (timeout != -1)
                {
                    buf += NVP_SEPARATOR + TIMEOUT + NV_SEPARATOR + timeout;
                }

                if (connectionLimit != -1)
                {
                    buf += NVP_SEPARATOR + CONNECTION_LIMIT + NV_SEPARATOR + connectionLimit;
                }

                if (EffectiveServerURL != null)
                {
                    buf += NVP_SEPARATOR + EFFECTIVE_SERVER_URL + NV_SEPARATOR + EffectiveServerURL;
                }

                return (buf);
            }
        }

        /// <summary>
        /// Returns the serverURL that will take effect given
        /// the current state of this Configuration object.
        /// </summary>
        internal string EffectiveServerURL
        {
            get
            {
                if (serverURL != null) return (serverURL);
                if (!isSendToProductionSet)
                {
                    throw new ApplicationException(
                        "CONFIGURATION BUG:  sendToProduction or serverURL must be specified!");
                }
                if (sendToProduction)
                {
                    return (sendToAkamai ? AKAMAI_PROD_URL:PROD_URL);
                }

                return (sendToAkamai ? AKAMAI_TEST_URL:TEST_URL);
            }
        }

        /// <summary>
        /// Returns the key filename that will take effect given
        /// the current state of this Configuration object.
        /// </summary>
        internal string EffectiveKeyFilename
        {
            get
            {
                return (
                    keyFilename != null 
                        ? keyFilename 
                        : merchantID + P12_EXTENSION );
            }
        }

        /// <summary>
        /// Return the key file path that will take effect given
        /// the current state of this Configuration object.
        /// </summary>
        internal string EffectiveKeyFilePath
        {
            get
            {
                return Path.Combine(KeysDirectory, EffectiveKeyFilename);
            }
        }

        /// <summary>
        /// Returns the password that will take effect given
        /// the current state of this Configuration object.
        /// </summary>
        internal string EffectivePassword
        {
            get
            {
                return( password != null ? password : merchantID);
            }
        }

        /// <summary>
        /// Returns the merchantID.  Throws an exception when it is null.
        /// </summary>
        public string NonNullMerchantID
        {
            get { CheckMerchantID();  return merchantID; }
        }

        private void CheckMerchantID()
        {
            if (merchantID == null)
            {
                throw new ApplicationException(
                    "CONFIGURATION OR CODE BUG:  merchantID is missing!");
            }
        }

        public bool UseSignedAndEncrypted
        {
            get { return useSignedAndEncrypted; }
            set { useSignedAndEncrypted = value; }
        }
    }
}
