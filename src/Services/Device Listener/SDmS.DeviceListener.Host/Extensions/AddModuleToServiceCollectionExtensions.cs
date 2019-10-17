using Microsoft.Extensions.DependencyInjection;

namespace SDmS.DeviceListener.Host.Extensions
{
    public static class AddModuleToServiceCollectionExtensions
    {
        public static void RegisterModule<T>(this IServiceCollection services) where T : IModule, new()
        {
            new T().Register(services);
        }
    }

    public interface IModule
    {
        void Register(IServiceCollection services);
    }
}
