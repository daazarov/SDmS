
namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceParameterValue
    {
        public string device_id { get; set; }
        public int parameter_id { get; set; }
        public string value { get; set; }

        public virtual Device Devices { get; set; }
        public virtual DeviceParameter Parameters { get; set; }
    }
}
