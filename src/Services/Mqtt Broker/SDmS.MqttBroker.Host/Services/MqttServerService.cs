using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using SDmS.MqttBroker.Host.MqttHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Services
{
    public class MqttServerService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IMqttServer _mqttServer;
        private IMqttServerOptions _options;

        public MqttServerService(ILogger<MqttServerService> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
            ConfigureServer();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mqttServer.StartAsync(_options);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttServer.StopAsync();
        }

        private void ConfigureServer()
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1883);

            optionsBuilder.WithStorage(new RetainedMessageHandler());

            // Setup client validator.
            optionsBuilder.WithConnectionValidator(c =>
            {
                c.ReasonCode = MqttConnectReasonCode.Success;
            });

            _options = optionsBuilder.Build();

            var logger = new MqttNetLogger();

            _mqttServer = new MqttFactory().CreateMqttServer();

            _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandler(logger);
            _mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandler(logger);
            _mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandler();
            _mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandler();
            _mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandler();
        }
    }
}
