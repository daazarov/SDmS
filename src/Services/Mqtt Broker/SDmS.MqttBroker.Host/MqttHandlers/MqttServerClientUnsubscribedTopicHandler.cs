using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.MqttHandlers
{
    public class MqttServerClientUnsubscribedTopicHandler : IMqttServerClientUnsubscribedTopicHandler
    {
        private readonly IMqttNetLogger _logger;

        public MqttServerClientUnsubscribedTopicHandler(IMqttNetLogger logger)
        {
            this._logger = logger;
        }

        public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            _logger.Publish(MqttNetLogLevel.Info, "", $"Client: {eventArgs.ClientId} unsubscribed topic!", null, null);
            return Task.FromResult<object>(null);
        }
    }
}
