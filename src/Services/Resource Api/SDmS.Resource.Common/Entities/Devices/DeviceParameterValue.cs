
using System;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceParameterValue
    {
        public string device_id { get; set; }
        public int parameter_id { get; set; }
        public string value { get; set; }
        public DateTime date_on { get; set; }

        public virtual Device Device { get; set; }
        public virtual DeviceParameter Parameter { get; set; }
    }
}
