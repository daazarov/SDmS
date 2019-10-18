using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using NServiceBus;
using SDmS.Messages.Common.Models.Enums;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.Mqtt.Handlers
{
    public class MqttApplicationMessageReceivedHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly ILogger _logger;
        private readonly Lazy<IMessageSession> _eventBusSession;
        private readonly MqttReceiverMessageFactory _messageFactory;

        public MqttApplicationMessageReceivedHandler(ILogger<MqttClient> logger, Lazy<IMessageSession> eventBusSession, MqttReceiverMessageFactory messageFactory)
        {
            this._eventBusSession = eventBusSession;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var session = _eventBusSession.Value;

            if (!_eventBusSession.IsValueCreated)
            {
                _logger.LogError($"ClientId: {eventArgs.ClientId}. Message in topic: {eventArgs.ApplicationMessage.Topic}. ERROR: NServiceBus session is not initialized.");
                eventArgs.ApplicationMessage.Retain = true;
                return;
            }

            var handler =_messageFactory.GetHandler(eventArgs.ApplicationMessage.Topic);
            if (handler == null)
            {
                eventArgs.ApplicationMessage.Retain = true;
                return;
            }

            switch (handler.Type)
            {
                case MessageType.Command:
                    var command = handler.ParseCommand(eventArgs);
                    await session.Send(command);
                    break;
                case MessageType.Event:
                    var @event = handler.ParseEvent(eventArgs);
                    await session.Publish(@event);
                    break;
            }
        }
    }
}
