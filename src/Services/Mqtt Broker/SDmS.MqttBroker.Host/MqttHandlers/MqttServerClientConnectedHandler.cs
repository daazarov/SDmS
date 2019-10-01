using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.MqttHandlers
{
    public class MqttServerClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private readonly IMqttNetLogger _logger;

        public MqttServerClientConnectedHandler(IMqttNetLogger logger)
        {
            this._logger = logger;
        }

        public Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            _logger.Publish(MqttNetLogLevel.Info, "", $"Client: {eventArgs.ClientId} connected!", null, null);
            return Task.FromResult<object>(null);
        }
    }
}
