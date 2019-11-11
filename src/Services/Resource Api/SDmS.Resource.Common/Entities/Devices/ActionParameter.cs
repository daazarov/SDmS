using System.Collections.Generic;

namespace SDmS.Resource.Common.Entities.Devices
{
    public class ActionParameter
    {
        public int action_parameter_id { get; set; }
        public string description { get; set; }
        public string required_flag { get; set; }

        public virtual ICollection<ActionParameterBinding> Bindings { get; set; }
    }
}
