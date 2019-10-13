using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers
{
    public class MqttApplicationMessageReceivedHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly ILogger _logger;
        private readonly Lazy<IMessageSession> _eventBusSession;

        public MqttApplicationMessageReceivedHandler(ILogger<MqttClient> logger, Lazy<IMessageSession> eventBusSession)
        {
            this._eventBusSession = eventBusSession;
            this._logger = logger;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
