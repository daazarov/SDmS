using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using SDmS.Messages.Common.Commands;
using SDmS.Messages.Common.Messages;
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

            var endpointConfiguration = new EndpointConfiguration(settings.RabbitEndPoint.Name);
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            transport.ConnectionString(settings.ConnectionString);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();

            var routing = transport.Routing();
            ConfigureRouting(routing);

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.EnableCallbacks();
			endpointConfiguration.MakeInstanceUniquelyAddressable("response-queue");

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(settings.RabbitEndPoint.ErrorQueue);

            return endpointConfiguration;
        }

        private static void ConfigureRouting(RoutingSettings<RabbitMQTransport> routing)
        {
            routing.RouteToEndpoint(typeof(DeviceDeleteCommand), "sdms.device-listener.host");
            routing.RouteToEndpoint(typeof(DeviceVerificationMessage), "sdms.device-listener.host");
            routing.RouteToEndpoint(typeof(DeviceAssignCommand), "sdms.device-listener.host");
            routing.RouteToEndpoint(typeof(DeviceCommandExecute), "sdms.device-enactor.host");
        }
    }
}
