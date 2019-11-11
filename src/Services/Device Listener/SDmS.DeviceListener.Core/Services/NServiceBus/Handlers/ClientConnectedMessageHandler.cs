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
            _logger.Info($"ClientConnectedMessage recived. DETAILS:\n{message.ToString()}");

            var devices = await this._mqttClientManager.GetDevicesByMqttClientAsync(message.client_id);

            foreach (var device in devices)
            {
                var @event = new DeviceConnectEvent
                {
                    mqtt_client_id = device.mqtt_client_id,
                    serial_number = device.serial_number,
                    type_text = device.type_text
                };

                await context.Publish(@event);
            }

            //await this._mqttClientManager.ChangeDevicesStatusAsync(true, message.client_id);
        }
    }
}
