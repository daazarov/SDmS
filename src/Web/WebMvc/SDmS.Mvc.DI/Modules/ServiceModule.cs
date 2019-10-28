using Ninject.Modules;
using Ninject.Web.Common;
using SDmS.Domain.Common.Identity;
using SDmS.Domain.Core.Interfases.Services;
using SDmS.Domain.Services;

namespace SDmS.Mvc.DI.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIdentityParser<ApplicationUser>>().To<IdentityParser>().InRequestScope();
            Bind<IMembershipService>().To<MembershipService>().InRequestScope();
            Bind<ILoggingService>().To<LoggingService>().InRequestScope();
            Bind<ILedDeviceService>().To<LedDeviceService>().InRequestScope();
            Bind<IClimateDeviceService>().To<ClimateDeviceService>().InRequestScope();
        }
    }
}
