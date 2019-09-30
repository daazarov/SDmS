using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ninject.Modules;
using Ninject.Web.Common;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Data.Context;
using SDmS.Identity.Core.Interfaces.Data;
using SDmS.Identity.Core.Interfaces.Services;
using SDmS.Identity.Core.Providers;
using SDmS.Identity.Core.Services;

namespace SDmS.Identity.DI.Modules
{
    public class IdentityModule : NinjectModule
    {
        private string _connectionString;
        public IdentityModule(string connection)
        {
            this._connectionString = connection;
        }

        public override void Load()
        {
            // Context
            Bind<IAppIdentityContext>().To<AppIdentityContext>().InRequestScope().WithConstructorArgument(_connectionString);

            // Services
            Bind<IIdentityEmailService>().To<EmailService>().InRequestScope();
            Bind<IIdentityInitializationService>().To<IdentityInitializationService>().InRequestScope();
            Bind<ILoggingService>().To<LoggingService>().InRequestScope();

            // Stores
            Bind(typeof(IUserStore<>)).To(typeof(UserStore<>))
                .InRequestScope()
                .WithConstructorArgument("context", _ => _.Kernel.GetService(typeof(IAppIdentityContext)));
            Bind<IRoleStore<ApplicationRole, string>>().To<RoleStore<ApplicationRole>>()
                .InRequestScope()
                .WithConstructorArgument("context", _ => _.Kernel.GetService(typeof(IAppIdentityContext)));

            Bind<IdentityFactoryOptions<ApplicationUserManager>>().ToMethod(GetTokenProvider).InRequestScope();

            // Managers
            Bind<IUserManager<ApplicationUser>>().To<ApplicationUserManager>().InRequestScope();
            Bind<IRoleManager<ApplicationRole>>().To<ApplicationRoleManager>().InRequestScope();
        }

        private IdentityFactoryOptions<ApplicationUserManager> GetTokenProvider(Ninject.Activation.IContext context)
        {
            return new IdentityFactoryOptions<ApplicationUserManager>()
            {
                // DpapiDataProtectionProvider not working in Azure web app
                // then implement MachineKeyProtectionProvider
                // https://stackoverflow.com/questions/23455579/generating-reset-password-token-does-not-work-in-azure-website/23661872#23661872
                DataProtectionProvider = new MachineKeyProtectionProvider()
            };
        }
    }
}
