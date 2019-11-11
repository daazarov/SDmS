namespace SDmS.Resource.Common.Entities.Devices
{
    public class ActionParameterBinding
    {
        public int action_id { get; set; }
        public int action_parameter_id { get; set; }

        public virtual Action Action { get; set; }
        public virtual ActionParameter Parameter { get; set; }
    }
}
