using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;
using SDmS.Identity.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SDmS.Identity.Api.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUserManager<ApplicationUser> _userManager;

        public CustomOAuthProvider()
        {
            this._userManager = (IUserManager<ApplicationUser>)NinjectHelper.GetResolveService(typeof(IUserManager<ApplicationUser>));
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Here we get the Custom Field sent in /oauth/token
            string[] is_remember = context.Parameters.Where(x => x.Key == "remember").Select(x => x.Value).FirstOrDefault();
            if (is_remember.Length > 0 && is_remember[0].Trim().Length > 0)
            {
                bool value;

                if (bool.TryParse(is_remember[0].Trim(), out value))
                {
                    context.OwinContext.Set<bool>("remember", value);
                }
                else
                {
                    context.OwinContext.Set<bool>("remember", false);
                }
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            ApplicationUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }

            if (context.OwinContext.Get<bool>("remember"))
            {
                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromDays(365);
            }
            else
            {
                context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromDays(1);
            }
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, Convert.ToString(user.Id)),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Firstname", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("JoinDate", user.JoinDate.ToString())
            };

            var oAuthIdentity = await _userManager.CreateIdentityAsync(user, "JWT");
            oAuthIdentity.AddClaims(claims);

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);
        }
    }
}