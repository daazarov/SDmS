using SDmS.Messages.Common.Models;
using System.Collections.Generic;

namespace SDmS.Messages.Common.Messages
{
    public class DeviceExistenceResponseMessage : DeviceMessage
    {
        public bool is_exist { get; set; }
        public DeviceInfo device_info { get; set; }
		public Dictionary<string, dynamic> parameters { get; set; }
    }

    public class DeviceInfo
    {
        public string mqtt_client_id { get; set; }
        public string type_text { get; set; }
        public bool is_online { get; set; }
    }
}
