namespace SDmS.Resource.Api.Models.Devices
{
    public class DeviceRequestModel
    {
        public string user_id { get; set; }
        public int? limit { get; set; }
        public int? offset { get; set; }
        public int type { get; set; }
    }
}
