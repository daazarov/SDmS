using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceType
    {
        public int device_type_id { get; set; }
        public string description { get; set; }
        public int device_category_id { get; set; }

        public virtual DeviceCategory Category { get; set; }
        public virtual ICollection<DeviceParameterBinding> Bindings { get; set; }
    }
}
