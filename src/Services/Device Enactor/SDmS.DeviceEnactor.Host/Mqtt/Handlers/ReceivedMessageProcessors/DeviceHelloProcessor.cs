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
        public override string MessageProcessorName => "BaseDeviceInitialiser";
        public override string TopicPattern => "devices/[0-9a-zA-Z]+/hello";
        public override MessageType Type => MessageType.DeviceEvent;

        public override DeviceEvent ParseDeviceEvent(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var payload = eventArgs.ApplicationMessage.Payload;
            if (payload == null || payload.Length == 0)
            {
                return null;
            }

            var payloadStr = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            if (this.IsValidJson(payloadStr))
            {
                DeviceHelloEvent message = JsonConvert.DeserializeObject<DeviceHelloEvent>(payloadStr);
                message.is_online = true;

                if (string.IsNullOrEmpty(message.mqtt_client_id)) message.mqtt_client_id = eventArgs.ClientId;

                return message;
            }
            return null;
        }
    }
}
