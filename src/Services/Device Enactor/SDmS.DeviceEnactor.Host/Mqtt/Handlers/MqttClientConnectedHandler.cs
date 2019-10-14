using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers
{
    public class MqttClientConnectedHandler : IMqttClientConnectedHandler
    {
        private readonly ILogger _logger;

        public MqttClientConnectedHandler(ILogger<MqttClient> logger)
        {
            this._logger = logger;
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            _logger.LogInformation($"Authenticate result: {eventArgs.AuthenticateResult.ResultCode.ToString()}");
            return Task.CompletedTask;
        }
    }
}
