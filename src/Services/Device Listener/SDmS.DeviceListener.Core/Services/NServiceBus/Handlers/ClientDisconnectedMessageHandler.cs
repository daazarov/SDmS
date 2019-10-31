using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class ClientDisconnectedMessageHandler : IHandleMessages<MqttClientDisconnectedEvent>
    {
        private readonly ILog _logger;
        private readonly IMqttClientManager _mqttClientManager;

        public ClientDisconnectedMessageHandler(IMqttClientManager mqttClientManager)
        {
            _logger = LogManager.GetLogger<ClientDisconnectedMessageHandler>();
            this._mqttClientManager = mqttClientManager;
        }

        public async Task Handle(MqttClientDisconnectedEvent message, IMessageHandlerContext context)
        {
            await this._mqttClientManager.ChangeDevicesStatusAsync(false, message.client_id);
        }
    }
}
