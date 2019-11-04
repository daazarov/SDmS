using System;

namespace SDmS.Resource.Domain.Models.Devices
{
    public class DeviceParameterValueDomain
    {
        public string device_id { get; set; }
        public int parameter_id { get; set; }
        public string parameter_name { get; set; }
        public string value { get; set; }
        public DateTime date_on { get; set; }
    }
}
