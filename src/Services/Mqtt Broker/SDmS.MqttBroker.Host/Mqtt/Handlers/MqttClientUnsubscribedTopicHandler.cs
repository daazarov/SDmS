using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttClientUnsubscribedTopicHandler : IMqttServerClientUnsubscribedTopicHandler
    {
        private readonly ILogger _logger;

        public MqttClientUnsubscribedTopicHandler(ILogger<MqttServer> logger)
        {
            this._logger = logger;
        }

        public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            _logger.LogInformation($"Client: {eventArgs.ClientId} unsubscribed topic {eventArgs.TopicFilter}");
            return Task.CompletedTask;
        }
    }
}
