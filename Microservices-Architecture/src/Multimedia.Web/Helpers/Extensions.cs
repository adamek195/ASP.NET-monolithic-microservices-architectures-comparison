using System.Linq;
using System.Security.Claims;

namespace Multimedia.Web.Helpers
{
    public static class Extensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var id = user.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            return id;
        }

        public static string GetToken(this ClaimsPrincipal user)
        {
            var token = user.Claims
                .Single(c => c.Type == "Token")
                .Value;

            return token;
        }
    }
}
