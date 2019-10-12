using Microsoft.Extensions.Logging;
using MQTTnet.Protocol;
using MQTTnet.Server;
using SDmS.MqttBroker.Host.Configuration;
using System;
using System.Threading.Tasks;

namespace SDmS.MqttBroker.Host.Mqtt.Handlers
{
    public class MqttServerConnectionValidator : IMqttServerConnectionValidator
    {
        private readonly ILogger _logger;
        private readonly MqttSettingsModel _settings;

        public MqttServerConnectionValidator(MqttSettingsModel settings, ILogger<MqttServerConnectionValidator> logger)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task ValidateConnectionAsync(MqttConnectionValidatorContext context)
        {
            if (context.Username != _settings.Username)
            {
                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                _logger.LogWarning($"Client_Id: {context.ClientId} - Invalid username or password");
                return Task.CompletedTask;
            }

            if (context.Password != _settings.Password)
            {
                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                _logger.LogWarning($"Client_Id: {context.ClientId} - Invalid username or password");
                return Task.CompletedTask;
            }

            context.ReasonCode = MqttConnectReasonCode.Success;

            return Task.CompletedTask;
        }
    }
}
