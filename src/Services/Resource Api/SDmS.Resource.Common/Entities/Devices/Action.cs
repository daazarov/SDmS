using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class Action
    {
        public int action_id { get; set; }
        public string description { get; set; }
		public int device_type_id { get; set; }

        public virtual DeviceType DeviceType { get; set; }
        public virtual ICollection<ActionParameterBinding> Bindings { get; set; }
    }
}
