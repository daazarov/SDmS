using SDmS.Messages.Common.Models;

namespace SDmS.Messages.Common.Commands
{
    public class DeviceDeleteCommand : DeviceCommand
    {
		public string type_text { get; set; }
		public string mqtt_client_id { get; set; }
    }
}
