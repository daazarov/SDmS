using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceCategory
    {
        public int device_category_id { get; set; }
        public string description { get; set; }

        public virtual ICollection<DeviceType> Types { get; set; }
    }
}
