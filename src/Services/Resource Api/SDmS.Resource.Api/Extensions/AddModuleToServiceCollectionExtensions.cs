using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDmS.Resource.DI;

namespace SDmS.Resource.Api.Extensions
{
    public static class AddModuleToServiceCollectionExtensions
    {
        public static void RegisterModule<T>(this IServiceCollection services, IConfiguration configuration) where T : IModule, new()
        {
            new T().Register(services, configuration);
        }
    }
}
