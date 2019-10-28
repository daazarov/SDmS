using SDmS.Resource.Common;
using SDmS.Resource.Domain.Interfaces.Services;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace SDmS.Resource.Domain.Services.Services
{
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        public ApplicationUser Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal /*claims*/)
            {
                var user = new ApplicationUser();

                var claims = principal as ClaimsPrincipal;

                if (claims.HasClaim(x => x.Type == ClaimTypes.Email))
                {
                    user.Email = claims.FindFirst(x => x.Type == ClaimTypes.Email).Value;
                }
                if (claims.HasClaim(x => x.Type == "FirstName"))
                {
                    user.FirstName = claims.FindFirst(x => x.Type == "FirstName").Value;
                }
                if (claims.HasClaim(x => x.Type == "LastName"))
                {
                    user.LastName = claims.FindFirst(x => x.Type == "LastName").Value;
                }
                if (claims.HasClaim(x => x.Type == ClaimTypes.Sid))
                {
                    user.id = claims.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
                }
                if (claims.HasClaim(x => x.Type == "JoinDate"))
                {
                    user.JoinDate = DateTime.Parse(claims.FindFirst(x => x.Type == "JoinDate").Value);
                }
                if (claims.HasClaim(x => x.Type == ClaimTypes.Name))
                {
                    user.UserName = claims.FindFirst(x => x.Type == ClaimTypes.Name).Value;
                }

                return user;
            }
            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
