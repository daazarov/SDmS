using SDmS.Messages.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Messages.Common.Commands
{
    public class DeviceCommandExecute : DeviceCommand
    {
        public string actionName { get; set; }
        public string mqtt_client_id { get; set; }
        public Dictionary<string, dynamic> parameters { get; set; }

        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command name: {type.FullName}");
            builder.AppendLine($"Action name: {actionName}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"MQTT Client Id: {mqtt_client_id}");
            builder.AppendLine("Parameters:");

            foreach (var parameter in parameters)
            {
                builder.AppendLine($"\t{parameter.Key}: {parameter.Value}");
            }

            return builder.ToString();
        }
    }
}
