using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.MqttHandlers
{
    public class MqttServerClientUnsubscribedTopicHandler : IMqttServerClientUnsubscribedTopicHandler
    {
        public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
