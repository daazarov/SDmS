using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceEnactor.Host.Configuration
{
    public class MqttSettingsModel
    {
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CommunicationTimeout { get; set; }
        public bool EnableCleanSessions { get; set; }
        public bool EnableDebugLogging { get; set; }
        public TcpEndPoint TcpEndPoint { get; set; } = new TcpEndPoint();
        public List<string> ListenerTopics { get; set; } = new List<string>();
    }
}
