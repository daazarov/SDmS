using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus.Logging;
using NServiceBus;
using System.Text;

namespace SDmS.MqttBroker.Host.Services
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
            try
            {
                _instance = await Endpoint.Start(_endpointConfiguration).ConfigureAwait(false);
                _logger.Info("NServiceBus service stared.");
            }
            catch (Exception ex)
            {
                var message = new StringBuilder(ex.Message);

                var inner = ex.InnerException;
                while (inner != null)
                {
                    message.Append(" \nINNER EXCEPTION: ");
                    message.Append(inner.Message);
                    inner = inner.InnerException;
                }

                _logger.Error(message.ToString());
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("NServiceBus service stoped.");
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
