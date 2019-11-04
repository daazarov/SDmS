using System;
using System.Collections.Generic;

namespace SDmS.Resource.Domain.Models.Devices
{
    public class DeviceDomainModel
    {
        public string device_id { get; set; }
        public string name { get; set; }
        public string serial_number { get; set; }
        public int device_type_id { get; set; }
        public string user_id { get; set; }
        public bool is_online { get; set; }
        public bool? is_enable { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime? last_receive_data_time { get; set; }

        public virtual ICollection<DeviceParameterValueDomain> DeviceParameters { get; set; }
    }
}
