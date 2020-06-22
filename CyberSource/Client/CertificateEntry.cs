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
        public DateTime CreationTime { get; set; }
        public X509Certificate2 MerchantCert { get; set; }
        public X509Certificate2 CybsCert { get; set; }
    }
}
