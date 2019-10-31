using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public abstract class MqttClientEvent : IEvent
    {
        public string client_id { get; set; }
    }
}
