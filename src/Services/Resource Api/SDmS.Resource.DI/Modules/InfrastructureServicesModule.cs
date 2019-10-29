using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Services;

namespace SDmS.Resource.DI.Modules
{
    public class InfrastructureServicesModule : IModule
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceBusSender,ServiceBusSender>();
        }
    }
}
