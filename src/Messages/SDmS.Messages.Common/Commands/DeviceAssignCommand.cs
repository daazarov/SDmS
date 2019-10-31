using SDmS.Messages.Common.Models;
using System.Collections.Generic;

namespace SDmS.Messages.Common.Commands
{
    public class DeviceAssignCommand : DeviceCommand
    {
		public string device_id { get; set; }
		public string mqtt_client_id { get; set; }
		public string type_text { get; set; }
		
		public Dictionary<string, dynamic> parameters { get; set; }
    }
}
