namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceParameterBinding
    {
        public int device_type_id { get; set; }
        public int parameter_id { get; set; }

        public virtual DeviceType Type { get; set; }
        public virtual DeviceParameter Parameter { get; set; }
    }
}
