using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SDmS.Resource.DI
{
    public interface IModule
    {
        void Register(IServiceCollection services, IConfiguration configuration);
    }
}
