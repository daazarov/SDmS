using System;
using System.Collections.Generic;
using System.Text;
using MQTTnet;
using Newtonsoft.Json;
using SDmS.Messages.Common.Messages;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers.ReceivedMessageProcessors
{
    public class TemperatureDataMessageProcessor : MqttMessageProcessor
    {
        public override string MessageProcessorName => "TemperatureReader";
        public override string TopicPattern => "devices/[0-9a-zA-Z]+/temperature/data";
        public override MessageType Type => MessageType.DeviceMessage;

        public override DeviceMessage ParseDeviceMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var payload = eventArgs.ApplicationMessage.Payload;
            if (payload == null || payload.Length == 0)
            {
                return null;
            }

            var payloadStr = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            if (this.IsValidJson(payloadStr))
            {
                TemperatureDataMessage message = JsonConvert.DeserializeObject<TemperatureDataMessage>(payloadStr);

                if (string.IsNullOrEmpty(message.mqtt_client_id)) message.mqtt_client_id = eventArgs.ClientId;

                return message;
            }
            return null;
        }
    }
}
