using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CyberSource.Clients
{
    /// <summary>
    /// Encapsulates retrieval of application settings.  You can modify
    /// this code to suit your needs.
    /// </summary>
    public class AppSettings
    {
        private const String CYBS_PREFIX = "cybs";
        private const String SEPARATOR = ".";
        private const string TRUE = "TRUE";
        private const string ONE = "1";

        /// <summary>
        /// Calls the other GetSetting() with a null default value.
        /// See its summary for details.
        /// </summary>
        public static string GetSetting(string merchantID, string key)
        {
            return (GetSetting(merchantID, key, null));
        }

        /// <summary>
        /// Gets the specified setting from the config file.
        /// 
        /// Search order is as follows:
        ///     cybs.value_of_merchantID.value_of_key
        ///     value_of_merchantID.value_of_key
        ///     cybs.value_of_key
        ///     value_of_key
        /// 
        /// The ones without the cybs prefix are supported
        /// only for backwards-compatibility.
        /// 
        /// You can specify merchantID-specific settings by
        /// prefixing them with the merchantID.  For example:
        /// cybs.merchant1.keysDir will apply only to merchant1.
        /// cybs.keysDir will apply to all other merchants.
        /// 
        /// Please note that the following settings cannot be
        /// merchant-specific:
        ///     merchantID
        ///     proxyURL
        ///     proxyUser
        ///     proxyPassword
        /// 
        /// There can therefore only be one set of proxy server settings
        /// for all merchant ids.  The reason being the Proxy object,
        /// if needed, is initialized during start-up.  It would be
        /// expensive to keep creating one for each request.
        /// 
        /// If merchantID is set in the request, it overrides the one
        /// in the config file.
        /// </summary>
        /// <param name="merchantID">
        /// merchant id whose key is being requested
        /// </param>
        /// <param name="key">
        /// key of the setting being requested
        /// </param>
        /// <param name="defaultVal">
        /// value returned if none is found in the config file.
        /// </param>
        /// <returns>
        /// value read from the config file, or the value of defaultVal
        /// if none was found.
        /// </returns>
        public static string GetSetting(
            string merchantID, string key, string defaultVal)
        {
            String val = null;

            if (merchantID != null)
            {
                // look for cybs.merchantID.key
                val = ConfigurationManager.AppSettings[
                        CYBS_PREFIX + SEPARATOR +
                        merchantID + SEPARATOR + key];

                if (val == null)
                {
                    // look for merchantID.key
                    val = ConfigurationManager.AppSettings[
                            merchantID + SEPARATOR + key];
                }
            }

            if (val == null)
            {
                // look for cybs.key
                val = ConfigurationManager.AppSettings[
                        CYBS_PREFIX + SEPARATOR + key];
            }

            if (val == null)
            {
                // look for key
                val = ConfigurationManager.AppSettings[key];
            }
            
            if (val == null)
            {
                // use default value
                val = defaultVal;
            }

            return (val);
        }

        /// <summary>
        /// Gets the specified boolean setting from the config file.
        /// </summary>
        /// <param name="merchantID">
        /// merchant id whose key is being requested
        /// </param>
        /// <param name="key">
        /// key of the setting being requested
        /// </param>
        /// <returns>
        /// 1 if the setting was found and was set to true or 1;
        /// 0 if the setting was found and was set to false or 0;
        /// -1 if the setting was not found.
        /// </returns>
        public static int GetBoolSetting(string merchantID, string key)
        {
            string val = GetSetting(merchantID, key);
            if (val != null)
            {
                return( ONE.Equals(val) || TRUE.Equals(val.ToUpper())
                        ? 1 : 0 );
            }
            else
            {
                return (-1);
            }
        }

        /// <summary>
        /// Gets the specified integer setting from the config file.
        /// </summary>
        /// <param name="merchantID">
        /// merchant id whose key is being requested
        /// </param>
        /// <param name="key">
        /// key of the setting being requested
        /// </param>
        /// <returns>
        /// integer value read from the config file.  A value of -1
        /// means the key was not found.
        /// </returns>
        public static int GetIntSetting(string merchantID, string key)
        {
            string val = GetSetting(merchantID, key);
            if (val != null)
            {
                // we'll let Int32.Parse() throw an exception for
                // non-numeric values.  It is a configuration bug!
                return(Int32.Parse(val));
            }
            else
            {
                return(-1);
            }
        }
    }
}
