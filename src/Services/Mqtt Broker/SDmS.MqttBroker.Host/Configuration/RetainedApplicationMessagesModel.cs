using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.MqttBroker.Host.Configuration
{
    public class RetainedApplicationMessagesModel
    {
        public bool Persist { get; set; } = false;

        public int WriteInterval { get; set; } = 10;

        public string Path { get; set; }
    }
}
