using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
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

            services.AddSignalR();
        }
    }

    public static class AddNServiceBusServiceCollectionExtensions
    {
        public static EndpointConfiguration AddNServiceBus(this IServiceCollection services, IConfiguration Configuration)
        {
            IEndpointInstance endpointInstance = null;
            services.AddSingleton<IMessageSession>(_ => endpointInstance);

            var endpointConfiguration = new EndpointConfiguration(Configuration["NServiceBus:EndPoint:Name"]);
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            transport.ConnectionString(Configuration["NServiceBus:ConnectionString"]);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.EnableUniformSession();

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(Configuration["NServiceBus:EndPoint:ErrorQueue"]);

            return endpointConfiguration;
        }
    }
}
