using System;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
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

            // Not working in Azure web app
            /*var dataProtectionProvider = _options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }*/


            //https://stackoverflow.com/questions/23455579/generating-reset-password-token-does-not-work-in-azure-website/30676983#30676983
            var provider = new MachineKeyProtectionProvider();
            this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                provider.Create("ResetPasswordPurpose"));
        }
    }

    public class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}
