using SDmS.Messages.Common.Models;

namespace SDmS.Messages.Common.Events
{
    public class DeviceHelloEvent : DevicePublishedEvent
    {
        public bool is_online { get; set; }
    }
}
