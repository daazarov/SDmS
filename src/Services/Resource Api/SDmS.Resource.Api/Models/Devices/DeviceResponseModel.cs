using System.Collections.Generic;

namespace SDmS.Resource.Api.Models.Devices
{
    public class DeviceResponseModel
    {
        public string device_id { get; set; }
        public string serial_number { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public string user_id { get; set; }

        public Dictionary<string, dynamic> parameters { get; set; }
    }
}
