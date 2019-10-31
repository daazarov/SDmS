namespace SDmS.Resource.Common.Entities.Devices
{
    public class DeviceAction
    {
        public string device_id { get; set; }
        public string action_id { get; set; }

        public virtual Device Device { get; set; }
        public virtual Action Action { get; set; }
    }
}
