using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Commands
{
    public class DeviceDeleteCommand : DeviceCommand
    {
		public string type_text { get; set; }
        public string device_id { get; set; }
		public string mqtt_client_id { get; set; }

        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command name: {type.FullName}");
            builder.AppendLine($"Device Id: {device_id}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"MQTT Client Id: {mqtt_client_id}");
            builder.AppendLine($"Device Type: {type_text}");

            return builder.ToString();
        }
    }
}
