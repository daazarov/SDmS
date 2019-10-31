using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class ClientConnectedMessageHandler : IHandleMessages<MqttClientConnectedEvent>
    {
        private readonly ILog _logger;
        private readonly IMqttClientManager _mqttClientManager;

        public ClientConnectedMessageHandler(IMqttClientManager mqttClientManager)
        {
            _logger = LogManager.GetLogger<ClientConnectedMessageHandler>();
            this._mqttClientManager = mqttClientManager;
        }

        public async Task Handle(MqttClientConnectedEvent message, IMessageHandlerContext context)
        {
            await this._mqttClientManager.ChangeDevicesStatusAsync(true, message.client_id);
        }
    }
}
