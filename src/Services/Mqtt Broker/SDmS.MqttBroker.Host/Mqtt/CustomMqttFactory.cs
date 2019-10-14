﻿using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using SDmS.MqttBroker.Host.Configuration;
using SDmS.MqttBroker.Host.Mqtt.Logging;
using System;

namespace SDmS.MqttBroker.Host.Mqtt
{
    public class CustomMqttFactory
    {
        private readonly MqttFactory _mqttFactory;

        public CustomMqttFactory(MqttSettingsModel settings, ILogger<MqttServer> logger)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            // It is important to avoid injecting the logger wrapper to ensure that no
            // unused log messages are generated by the MQTTnet library. Debug logging
            // has a huge performance impact.
            if (settings.EnableDebugLogging)
            {
                var mqttNetLogger = new MqttNetLoggerWrapper(logger);
                _mqttFactory = new MqttFactory(mqttNetLogger);

                logger.LogWarning("Debug logging is enabled. Performance of MQTTnet Server is decreased!");
            }
            else
            {
                _mqttFactory = new MqttFactory();
            }

            Logger = _mqttFactory.DefaultLogger;

        }

        public IMqttNetLogger Logger { get; }

        public IMqttServer CreateMqttServer()
        {
            return _mqttFactory.CreateMqttServer();
        }
    }
}