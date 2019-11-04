namespace SDmS.Messages.Common.Models
{
    public class DevicePublishedMessage : DeviceMessage
    {
        public string mqtt_client_id { get; set; }
        public string type { get; set; }
    }
}
