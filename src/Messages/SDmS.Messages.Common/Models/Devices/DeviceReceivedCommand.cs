using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Messages.Common.Models
{
    public class DeviceReceivedCommand : DeviceCommand
    {
        public string client_id { get; set; }
    }
}
