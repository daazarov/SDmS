namespace SDmS.DeviceListener.Host.Configuration
{
    public class BusSettingsModel
    {
        public string ConnectionString { get; set; }

        public RabbitEndPointModel RabbitEndPoint { get; set; } = new RabbitEndPointModel();
    }
}
