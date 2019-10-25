using MQTTnet;
using Newtonsoft.Json;
using SDmS.DeviceEnactor.Host.Mqtt.Handlers;
using SDmS.Messages.Common.Events;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;
using System;
using System.Text;

namespace SDmS.DeviceEnactor.Host.Mqtt.ReceivedMessageProcessors
{
    public class DeviceHelloProcessor : MqttMessageProcessor
    {
        public override string HandlerName => "BaseDeviceInitialiser";
        public override string TopicPattern => "devices/[0-9a-zA-Z]+/hello";
        public override MessageType Type => MessageType.Event;

        public override DeviceEvent ParseEvent(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var payload = eventArgs.ApplicationMessage.Payload;
            if (payload == null || payload.Length == 0)
            {
                return null;
            }

            var payloadStr = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            if (this.IsValidJson(payloadStr))
            {
                DeviceHelloMessage message = JsonConvert.DeserializeObject<DeviceHelloMessage>(payloadStr);

                if (string.IsNullOrEmpty(message.client_id)) message.client_id = eventArgs.ClientId;

                return message;
            }
            return null;
        }
    }
}
