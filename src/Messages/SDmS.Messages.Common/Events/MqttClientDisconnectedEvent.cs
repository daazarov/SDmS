using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Events
{
    public class MqttClientDisconnectedEvent : MqttClientEvent
    {
        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Event name: {type.FullName}");
            builder.AppendLine($"MQTT Client Id: {this.client_id}");

            return builder.ToString();
        }
    }
}
