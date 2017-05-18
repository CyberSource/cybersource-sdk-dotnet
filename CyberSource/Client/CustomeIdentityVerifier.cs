using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace CyberSource.Clients
{
    class CustomeIdentityVerifier : IdentityVerifier
    {
        IdentityVerifier defaultVerifier;

        public CustomeIdentityVerifier()
        {
            this.defaultVerifier = CustomeIdentityVerifier.CreateDefault();
        }

        public override bool CheckAccess(EndpointIdentity identity,
            AuthorizationContext authContext)
        {
            bool returnvalue = false;
            foreach (ClaimSet claimset in authContext.ClaimSets)
            {
                foreach (Claim claim in claimset)
                {
                    if (claim.ClaimType == BaseClient.X509_CLAIMTYPE)
                    {
                        X500DistinguishedName name = (X500DistinguishedName)claim.Resource;
                        if (name.Name.Contains(BaseClient.CYBERSOURCE_PUBLIC_KEY))
                        {
                            returnvalue = true;
                            break;
                        }
                    }
                }

            }
            // The following implementation is for demonstration only, and
            // does not perform any checks regarding EndpointIdentity.
            // Do not use this for production code.
            return returnvalue;
        }

        public override bool TryGetIdentity(EndpointAddress reference,
            out EndpointIdentity identity)
        {
            return this.defaultVerifier.TryGetIdentity(reference, out identity);
        }
    }
}
