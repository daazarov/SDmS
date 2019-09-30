using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus.Logging;
using NServiceBus;

namespace Reservation
{
    public class EventBusService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILog _logger;

        private IEndpointInstance _instance;
        private EndpointConfiguration _endpointConfiguration;

        public EventBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = LogManager.GetLogger<EventBusService>();
            Init();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Service stared.");
            _instance = await Endpoint.Start(_endpointConfiguration).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Service stoped.");
            await _instance.Stop();
        }

        private void Init()
        {
            _endpointConfiguration = new EndpointConfiguration(_configuration["NServiceBus:EndPoint:Name"]);

            var transport = _endpointConfiguration.UseTransport<RabbitMQTransport>();

            string connectionString = _configuration["NServiceBus:ConnectionString"];

            transport.ConnectionString(connectionString);
            transport.UsePublisherConfirms(true);
            transport.UseDirectRoutingTopology();
            _endpointConfiguration.EnableInstallers();

            _endpointConfiguration.EnableUniformSession();

            _endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            _endpointConfiguration.UsePersistence<InMemoryPersistence>();
            _endpointConfiguration.SendFailedMessagesTo(_configuration["NServiceBus:EndPoint:ErrorQueue"]);
        }
    }
}
