using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Messages.Common.Models
{
    public class DeviceReceivedEventMessage : DeviceEvent
    {
        public string type { get; set; }
    }
}
