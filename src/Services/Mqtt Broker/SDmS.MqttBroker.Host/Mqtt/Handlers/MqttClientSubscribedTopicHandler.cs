using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttClientSubscribedTopicHandler : IMqttServerClientSubscribedTopicHandler
    {
        private readonly ILogger _logger;

        public MqttClientSubscribedTopicHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} subscribed to {eventArgs.TopicFilter.Topic} topic!");
            return Task.CompletedTask;
        }
    }
}
