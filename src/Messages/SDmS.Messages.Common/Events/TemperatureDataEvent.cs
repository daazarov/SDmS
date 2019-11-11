using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Events
{
    public class TemperatureDataEvent : DevicePublishedEvent
    {
        public string temperature_data { get; set; }

        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command name: {type.FullName}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"Temperature Data: {temperature_data}");

            return builder.ToString();
        }
    }
}
