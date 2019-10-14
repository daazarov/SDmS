using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public abstract class DeviceEvent : IEvent
    {
        public string client_id { get; set; }
        public string serial_number { get; set; }
    }
}
