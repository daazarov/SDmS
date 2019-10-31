using System.Text;
using MQTTnet;
using SDmS.Messages.Common.Events;
using SDmS.Messages.Common.Models;
using SDmS.Messages.Common.Models.Enums;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers.ReceivedMessageProcessors
{
    public class ClientConnectedMessageProcessor : MqttMessageProcessor
    {
        public override string MessageProcessorName => "ClientConnectProcessor";

        public override string TopicPattern => @"clients/[0-9a-zA-Z_]+/connect";

        public override MessageType Type => MessageType.ClientEvent;

        public override MqttClientEvent ParseClientEvent(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            string payload = (eventArgs.ApplicationMessage.Payload.Length > 0) ? Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload) : "";

            if (string.IsNullOrEmpty(payload)) return null;

            return new MqttClientConnectedEvent { client_id = payload };
        }
    }
}
