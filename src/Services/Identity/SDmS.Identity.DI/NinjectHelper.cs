using Ninject;
using SDmS.Identity.DI.Modules;
using System;

namespace SDmS.Identity.DI
{
    public static class NinjectHelper
    {
        private static IKernel _kernel;
        public static IKernel GetNinjectResolver()
        {
            _kernel = new StandardKernel(new IdentityModule("IdentityConnection"));

            return _kernel;
        }

        public static object GetResolveService(Type typeService)
        {
            return (typeService != null)
                ? _kernel.GetService(typeService)
                : null;
        }
    }
}
