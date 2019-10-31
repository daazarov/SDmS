using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using SDmS.MqttBroker.Host.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private readonly ILogger _logger;
        private MqttServerService _mqttServer;

        public MqttServerService MqttServer
        {
            get { return _mqttServer; }
            set { _mqttServer = value; }
        }

        public MqttClientConnectedHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} connected!");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"clients/{eventArgs.ClientId}/connect")
                .WithPayload(eventArgs.ClientId)
                .WithExactlyOnceQoS()
                .WithContentType("client_event")
                .Build();

            await this._mqttServer.PublishAsync(message);
        }
    }
}
