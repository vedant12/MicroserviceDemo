using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Mango.Web.Utility
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity!;

            identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList().ForEach(c =>
            {
                identity.AddClaim(new Claim("role", c.Value));
            });

            return Task.FromResult(principal);
        }
    }
}
