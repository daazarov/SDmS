using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.MqttHandlers
{
    public class MqttServerClientSubscribedTopicHandler : IMqttServerClientSubscribedTopicHandler
    {
        private readonly IMqttNetLogger _logger;

        public MqttServerClientSubscribedTopicHandler(IMqttNetLogger logger)
        {
            this._logger = logger;
        }

        public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            _logger.Publish(MqttNetLogLevel.Info, "", $"Client: {eventArgs.ClientId} subscribed to {eventArgs.TopicFilter.Topic} topic!", null, null);
            return Task.FromResult<object>(null);
        }
    }
}
