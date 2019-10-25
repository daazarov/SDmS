using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using SDmS.Resource.Api.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDmS.Resource.Api.Extensions
{
    public static class AddTransportServiceCollection
    {
        public static void AddTransport(this IServiceCollection services)
        {
            services.AddCors(e =>
            {
                e.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(host => true));
            });
        }
    }

    public static class AddNServiceBusServiceCollectionExtensions
    {
        public static EndpointConfiguration AddNServiceBus(this IServiceCollection services, IConfiguration Configuration)
        {
            var settings = new BusSettingsModel();
            Configuration.Bind("NServiceBus", settings);

            IEndpointInstance endpointInstance = null;
            services.AddSingleton<IMessageSession>(_ => endpointInstance);

            var endpointConfiguration = new EndpointConfiguration(settings.RabbitEndPoint.Name);
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            transport.ConnectionString(Configuration[settings.ConnectionString]);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.EnableUniformSession();

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(Configuration[settings.RabbitEndPoint.ErrorQueue]);

            return endpointConfiguration;
        }
    }
}
