using System;
using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class Device
    {
        public int device_id { get; set; }
        public string name { get; set; }
        public string serial_number { get; set; }
        public int device_type_id { get; set; }
        public string user_id { get; set; }
        public string mqtt_client_id { get; set; }
        public bool is_online { get; set; }
        public bool? is_enable { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime? last_receive_data_time { get; set; }

        public virtual ICollection<DeviceParameterValue> DeviceParameters { get; set; }
        public virtual DeviceType Type { get; set; }
    }
}
