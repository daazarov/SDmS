using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDmS.Resource.Common;
using SDmS.Resource.Domain.Interfaces.Services;
using SDmS.Resource.Domain.Services;

namespace SDmS.Resource.DI.Modules
{
    public class DomainServicesModule : IModule
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IErrorInformatorService, ErrorInformatorService>();
            services.AddScoped<IIdentityParser<ApplicationUser>, IdentityParser>();
            services.AddScoped<IDeviceService, DeviceService>();
        }
    }
}
