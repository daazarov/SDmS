using NServiceBus;

namespace SDmS.Messages.Common.Models
{
    public class DeviceCommand : ICommand
    {
        public string client_id { get; set; }
        public string serial_number { get; set; }
    }
}
