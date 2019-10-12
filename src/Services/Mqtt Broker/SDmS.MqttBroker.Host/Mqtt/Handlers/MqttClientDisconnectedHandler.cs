using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly ILogger _logger;

        public MqttClientDisconnectedHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} disconnected!");
            return Task.CompletedTask;
        }
    }
}
