using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public abstract class DeviceEvent : IEvent
    {
        public string serial_number { get; set; }
    }
}
