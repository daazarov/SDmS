using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceParameter
    {
        public int parameter_id { get; set; }
        public int description { get; set; }

        public virtual ICollection<DeviceParameterValue> Values { get; set; }
    }
}
