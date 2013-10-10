using System.Linq;
using System.Security.Claims;

namespace ConsoleApplication1
{
    public class CustomAuthorisationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            string resource = context.Resource.First().Value;
            string action = context.Action.First().Value;

            if (action == "Show" && resource == "Code")
            {
                bool likesJava = context.Principal.HasClaim("http://www.mysite.com/likesjavatoo", "True");
                return likesJava;
            }

            return false;
        }
    }
}