using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Ninject.Modules;
using Ninject.Web.Common;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Data.Context;
using SDmS.Identity.Core.Interfaces.Data;
using SDmS.Identity.Core.Interfaces.Services;
using SDmS.Identity.Core.Services;
using System.Reflection;

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
            Bind<IAppIdentityContext>().To<AppIdentityContext>().InTransientScope().WithConstructorArgument(_connectionString);

            // Managers
            Bind<IUserManager<ApplicationUser>>().To<ApplicationUserManager>().InTransientScope();
            Bind<IRoleManager<ApplicationRole>>().To<ApplicationRoleManager>().InTransientScope();

            // Services
            Bind<IIdentityEmailService>().To<EmailService>().InSingletonScope();
            Bind<IIdentityInitializationService>().To<IdentityInitializationService>().InRequestScope();

            // Stores
            Bind(typeof(IUserStore<>)).To(typeof(UserStore<>))
                .InRequestScope()
                .WithConstructorArgument("context", this.Kernel.GetService(typeof(IAppIdentityContext)));
            Bind<IRoleStore<ApplicationRole, string>>().To<RoleStore<ApplicationRole>>()
                .InRequestScope()
                .WithConstructorArgument("context", this.Kernel.GetService(typeof(IAppIdentityContext)));

            Bind<IdentityFactoryOptions<ApplicationUserManager>>().ToMethod(GetTokenProvider).InRequestScope();
        }

        private IdentityFactoryOptions<ApplicationUserManager> GetTokenProvider(Ninject.Activation.IContext context)
        {
            return new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new DpapiDataProtectionProvider(Assembly.GetExecutingAssembly().GetName().Name)
            };
        }
    }
}
