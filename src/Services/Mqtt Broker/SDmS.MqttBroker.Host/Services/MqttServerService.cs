using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using SDmS.MqttBroker.Host.MqttHandlers;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Services
{
    public class MqttServerService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMqttNetLogger _mqttLogger;
        private IMqttServer _mqttServer;
        private IMqttServerOptions _options;

        public MqttServerService(ILogger<MqttServerService> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._mqttLogger = new MqttNetLogger("Server");
            this._mqttLogger.LogMessagePublished += _Logger_Message_Publish;
            ConfigureServer();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _mqttServer.StartAsync(_options);
                _logger.LogInformation("MQTT Broker started...");
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

                _logger.LogError(message.ToString());
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttServer.StopAsync();
        }

        private void ConfigureServer()
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(int.Parse(_configuration["MQTTPort"]));

            optionsBuilder.WithStorage(new RetainedMessageHandler());

            // Setup client validator.
            optionsBuilder.WithConnectionValidator(c =>
            {
                c.ReasonCode = MqttConnectReasonCode.Success;
            });

            _options = optionsBuilder.Build();

            _mqttServer = new MqttFactory().CreateMqttServer();

            _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandler(_mqttLogger);
            _mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandler(_mqttLogger);
            _mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandler(_mqttLogger);
            _mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandler(_mqttLogger);
            _mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandler(_mqttLogger);
        }

        private void _Logger_Message_Publish(object e, MqttNetLogMessagePublishedEventArgs args)
        {
            switch (args.TraceMessage.Level)
            {
                case MqttNetLogLevel.Info:
                    _logger.LogInformation(args.TraceMessage.Message);
                    break;
                case MqttNetLogLevel.Error:
                    _logger.LogError(args.TraceMessage.Exception, args.TraceMessage.Message);
                    break;
                case MqttNetLogLevel.Warning:
                    _logger.LogWarning(args.TraceMessage.Message);
                    break;
                case MqttNetLogLevel.Verbose:
                    _logger.LogDebug(args.TraceMessage.Message);
                    break;
            }
            
        }
    }
}
