using System.Text;
using MQTTnet;
using Newtonsoft.Json;
using SDmS.Messages.Common.Events;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers.ReceivedMessageProcessors
{
    public class DeviceConnectedEventProcessor : MqttMessageProcessor
    {
        public override string MessageProcessorName => "DeviceConnectProcessor";

        public override string TopicPattern => "devices/[0-9a-zA-Z]+/connect";

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
                DeviceConnectEvent message = JsonConvert.DeserializeObject<DeviceConnectEvent>(payloadStr);

                if (string.IsNullOrEmpty(message.mqtt_client_id)) message.mqtt_client_id = eventArgs.ClientId;

                return message;
            }
            return null;
        }
    }
}
