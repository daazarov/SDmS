namespace SDmS.DeviceEnactor.Host.Configuration
{
    public class RabbitEndPointModel
    {
        public string Name { get; set; }

        public string ErrorQueue { get; set; } = "error";
    }
}
