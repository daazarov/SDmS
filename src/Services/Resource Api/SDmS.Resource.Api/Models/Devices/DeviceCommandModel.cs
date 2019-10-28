using System.Collections.Generic;

namespace SDmS.Resource.Api.Models.Devices
{
    public class DeviceCommandModel
    {
        public string action_name { get; set; }
        public int type { get; set; }

        public Dictionary<string, dynamic> parameters { get; set; }
    }
}
