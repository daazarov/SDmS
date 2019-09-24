using SDmS.Domain.Common.Identity;
using SDmS.Domain.Core.Interfases.Services;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace SDmS.Domain.Services
{
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        public ApplicationUser Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal /*claims*/)
            {
                return new ApplicationUser
                {
                };
            }
            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
