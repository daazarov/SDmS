using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public class DeviceCommand : ICommand
    {
        public string serial_number { get; set; }
    }
}
