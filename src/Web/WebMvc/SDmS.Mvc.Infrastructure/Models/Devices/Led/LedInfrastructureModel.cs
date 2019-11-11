
using System.Collections.Generic;

namespace SDmS.Infrastructure.Models.Devices.Led
{
    public class LedInfrastructureModel
    {
        public string device_id { get; set; }
        public string name { get; set; }
        public string serial_number { get; set; }
        public bool is_online { get; set; }
        public string user_id { get; set; }

        public Dictionary<string, dynamic> parameters { get; set; }
    }
}
