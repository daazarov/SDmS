namespace SDmS.Domain.Core.Models.Climate
{
    public class TempControlDomainModel
    {
        public string serial_number { get; set; }
        public string name { get; set; }
        public bool is_online { get; set; }
        public double temp_c { get; set; }
        public bool is_control_enable { get; set; }
        public int desired_temp { get; set; }
    }
}
