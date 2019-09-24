using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using SDmS.Mvc.DI.Modules;
using System.Web.Mvc;

namespace SDmS.Mvc.App_Start
{
    public class IoCConfig
    {
        public static void RegisterServices()
        {
            NinjectModule services = new ServiceModule();

            var kernel = new StandardKernel(services);
            kernel.Unbind<ModelValidatorProvider>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}