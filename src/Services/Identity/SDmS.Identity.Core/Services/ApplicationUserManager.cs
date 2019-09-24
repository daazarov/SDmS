using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;

namespace SDmS.Identity.Core.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IUserManager<ApplicationUser>
    {
        private readonly IIdentityEmailService _emailService;
        private readonly IdentityFactoryOptions<ApplicationUserManager> _options;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IIdentityEmailService emailService, IdentityFactoryOptions<ApplicationUserManager> options) 
            : base(store)
        {
            this._emailService = emailService;
            this._options = options;

            Init();
        }

        private void Init()
        {
            //Config validators
            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            this.EmailService = _emailService;

            var dataProtectionProvider = _options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }
        }
    }
}
