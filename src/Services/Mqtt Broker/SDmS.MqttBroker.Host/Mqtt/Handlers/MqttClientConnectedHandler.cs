using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private readonly ILogger _logger;

        public MqttClientConnectedHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} connected!");
            return Task.CompletedTask;
        }
    }
}
