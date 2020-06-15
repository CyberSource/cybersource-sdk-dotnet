using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace CyberSource.Clients
{
    /// <summary>
    /// Encapsulates retrieval of application settings.  You can modify
    /// this code to suit your needs.
    /// </summary>
    public class CertificateEntry
    {
        private long lastModifiedTime;
        private X509Certificate2 merchantCert;
        private X509Certificate2 cybsCert;

        public CertificateEntry(long lastModifiedTime, X509Certificate2 merchantCert, X509Certificate2 cybsCert)
        {
            this.lastModifiedTime = lastModifiedTime;
            this.merchantCert = merchantCert;
            this.cybsCert = cybsCert;
        }

        public X509Certificate2 MerchantCert
        {
            get { return merchantCert; }
            set
            {
                merchantCert = value;
             
            }
        }

        public X509Certificate2 CybsCert
        {
            get { return cybsCert; }
            set
            {
                cybsCert = value;

            }
        }

        public long LastModifiedTime
        {
            get { return lastModifiedTime; }
            set
            {
                lastModifiedTime = value;

            }
        }
    }


}
