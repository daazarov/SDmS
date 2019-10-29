using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Resource.Domain.Models.Devices
{
    public class DeviceAddDomainModel
    {
        public string name { get; set; }
        public int type { get; set; }
        public string serial_number { get; set; }
    }
}
