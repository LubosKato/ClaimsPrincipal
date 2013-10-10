using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;

namespace ConsoleApplication1
{
    public class CustomClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            //validate name claim
            string nameClaimValue = incomingPrincipal.Identity.Name;

            if (string.IsNullOrEmpty(nameClaimValue))
            {
                throw new SecurityException("A user with no name???");
            }

            return CreatePrincipal(nameClaimValue);
        }

        private ClaimsPrincipal CreatePrincipal(string userName)
        {
            bool likesJavaToo = userName.IndexOf(System.Environment.MachineName, StringComparison.InvariantCultureIgnoreCase) > -1;

            List<Claim> claimsCollection = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName)
                ,
                new Claim("http://www.mysite.com/likesjavatoo", likesJavaToo.ToString())
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claimsCollection, "Custom"));
        }
    }
}