using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Events
{
    public class DeviceDisconnectEvent : DevicePublishedEvent
    {
        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Event name: {type.FullName}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"Device Type: {this.type_text}");

            return builder.ToString();
        }
    }
}
