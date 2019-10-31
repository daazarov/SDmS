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
    public class MqttClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly ILogger _logger;
        private MqttServerService _mqttServer;

        public MqttServerService MqttServer
        {
            get { return _mqttServer; }
            set { _mqttServer = value; }
        }

        public MqttClientDisconnectedHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} disconnected!");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"clients/{eventArgs.ClientId}/disconnect")
                .WithPayload(eventArgs.ClientId)
                .WithExactlyOnceQoS()
                .WithContentType("client_event")
                .Build();

            await this._mqttServer.PublishAsync(message);
        }
    }
}
