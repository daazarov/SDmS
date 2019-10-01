using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.MqttHandlers
{
    public class MqttServerClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly IMqttNetLogger _logger;

        public MqttServerClientDisconnectedHandler(IMqttNetLogger logger)
        {
            this._logger = logger;
        }

        public Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            _logger.Publish(MqttNetLogLevel.Info, "", $"Client: {eventArgs.ClientId} disconnected!", null, null);
            return Task.FromResult<object>(null);
        }
    }
}
