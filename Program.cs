using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Security;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SetCurrentPrincipal();
            UseCurrentPrincipal();

            Setup();
            CheckCompatibility();
            CheckNewClaimsUsage();

            Console.ReadLine();
        }

        private static void UseCurrentPrincipal()
        {
            try
            {
                ShowMeTheCode();
            }
            catch (SecurityException)
            {
            }
        }

        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "Show", Resource = "Code")]
        private static void ShowMeTheCode()
        {
            Console.WriteLine("Console.WriteLine");
        }

        private static void SetCurrentPrincipal()
        {
            WindowsPrincipal incomingPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            Thread.CurrentPrincipal = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .ClaimsAuthenticationManager.Authenticate("none", incomingPrincipal);
        }

        private static void CheckNewClaimsUsage()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            Claim nameClaim = currentClaimsPrincipal.FindFirst(ClaimTypes.Name);
            Console.WriteLine(nameClaim.Value);
        }

        private static void CheckCompatibility()
        {
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            Console.WriteLine(currentPrincipal.Identity.Name);
            Console.WriteLine(currentPrincipal.IsInRole("IT"));
        }

        private static void Setup()
        {
            IList<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Lubos")
                , new Claim(ClaimTypes.Country, "Slovakia")
                , new Claim(ClaimTypes.Gender, "M")
                , new Claim(ClaimTypes.Surname, "Kato")
                , new Claim(ClaimTypes.Email, "hello@me.com")
                , new Claim(ClaimTypes.Role, "IT")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimCollection, "My e-commerce website");

            Console.WriteLine(claimsIdentity.IsAuthenticated);

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = principal;
        }
    }
}
