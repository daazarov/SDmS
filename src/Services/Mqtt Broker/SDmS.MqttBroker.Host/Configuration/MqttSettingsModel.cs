namespace SDmS.MqttBroker.Host.Configuration
{
    public class MqttSettingsModel
    {
        /// <summary>
        /// MQTT username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// MQTT password
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Set default connection timeout in seconds
        /// </summary>
        public int CommunicationTimeout { get; set; } = 15;

        /// <summary>
        /// Set 0 to disable connection backlogging
        /// </summary>
        public int ConnectionBacklog { get; set; }

        /// <summary>
        /// Enable support for persistent sessions
        /// </summary>
        public bool EnablePersistentSessions { get; set; } = false;

        /// <summary>
        /// Listen Settings
        /// </summary>
        public TcpEndPointModel TcpEndPoint { get; set; } = new TcpEndPointModel();


        /// <summary>
        /// Set limit for max pending messages per client
        /// </summary>
        public int MaxPendingMessagesPerClient { get; set; } = 250;

        /// <summary>
        /// The settings for retained messages.
        /// </summary>
        public RetainedApplicationMessagesModel RetainedApplicationMessages { get; set; } = new RetainedApplicationMessagesModel();

        /// <summary>
        /// Enables or disables the MQTTnet internal logging.
        /// </summary>
        public bool EnableDebugLogging { get; set; } = false;
    }
}
