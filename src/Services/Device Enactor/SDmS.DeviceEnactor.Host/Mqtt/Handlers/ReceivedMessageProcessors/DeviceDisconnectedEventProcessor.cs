using System.Text;
using MQTTnet;
using Newtonsoft.Json;
using SDmS.Messages.Common.Events;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers.ReceivedMessageProcessors
{
    public class DeviceDisconnectedEventProcessor : MqttMessageProcessor
    {
        public override string MessageProcessorName => "DeviceDisconnectProcessor";

        public override string TopicPattern => "devices/[0-9a-zA-Z]+/disconnect";

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
                DeviceDisconnectEvent message = JsonConvert.DeserializeObject<DeviceDisconnectEvent>(payloadStr);

                if (string.IsNullOrEmpty(message.mqtt_client_id)) message.mqtt_client_id = eventArgs.ClientId;

                return message;
            }
            return null;
        }
    }
}
