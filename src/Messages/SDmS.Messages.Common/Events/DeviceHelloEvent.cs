﻿using SDmS.Messages.Common.Models;
using System.Text;

namespace SDmS.Messages.Common.Events
{
    public class DeviceHelloEvent : DevicePublishedEvent
    {
        public bool is_online { get; set; }

        public override string ToString()
        {
            var type = GetType();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Command name: {type.FullName}");
            builder.AppendLine($"Serial Number: {serial_number}");
            builder.AppendLine($"Device Type: {type}");

            return builder.ToString();
        }
    }
}
