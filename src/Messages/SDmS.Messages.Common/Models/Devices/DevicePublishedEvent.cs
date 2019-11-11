using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Messages.Common.Models
{
    public class DevicePublishedEvent : DeviceEvent
    {
        public string mqtt_client_id { get; set; }
        public string type_text { get; set; }
    }
}
