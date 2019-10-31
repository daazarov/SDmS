using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceParameter
    {
        public int parameter_id { get; set; }
        public string description { get; set; }
        public string default_value { get; set; }
        public string processing_flag { get; set; }

        public virtual ICollection<DeviceParameterValue> Values { get; set; }
        public virtual ICollection<DeviceParameterBinding> Bindings { get; set; }
    }
}
