using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using System;
using System.Threading;

namespace SDmS.DeviceEnactor.Host.Mqtt.Logging
{
    public class MqttNetLoggerWrapper : IMqttNetLogger
    {
        private readonly ILogger<MqttClient> _logger;

        public MqttNetLoggerWrapper(ILogger<MqttClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public event EventHandler<MqttNetLogMessagePublishedEventArgs> LogMessagePublished;

        public IMqttNetChildLogger CreateChildLogger(string source = null)
        {
            return new MqttNetChildLoggerWrapper(source, this);
        }

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            var convertedLogLevel = ConvertLogLevel(logLevel);
            _logger.Log(convertedLogLevel, exception, message, parameters);

            var logMessagePublishedEvent = LogMessagePublished;
            if (logMessagePublishedEvent != null)
            {
                var logMessage = new MqttNetLogMessage(null, DateTime.UtcNow, Thread.CurrentThread.ManagedThreadId, source, logLevel, message, exception);
                logMessagePublishedEvent.Invoke(this, new MqttNetLogMessagePublishedEventArgs(logMessage));
            }
        }

        private static LogLevel ConvertLogLevel(MqttNetLogLevel logLevel)
        {
            switch (logLevel)
            {
                case MqttNetLogLevel.Error: return LogLevel.Error;
                case MqttNetLogLevel.Warning: return LogLevel.Warning;
                case MqttNetLogLevel.Info: return LogLevel.Information;
                case MqttNetLogLevel.Verbose: return LogLevel.Debug;
            }

            return LogLevel.Debug;
        }
    }
}
