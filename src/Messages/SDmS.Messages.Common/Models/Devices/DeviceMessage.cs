using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public class DeviceMessage : IMessage
    {
        public string serial_number { get; set; }
    }
}
