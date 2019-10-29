namespace SDmS.Domain.Core.Models
{
    public class DeviceAddToUserDomainModel
    {
        public string serial_number { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public int type { get; set; }
    }
}
