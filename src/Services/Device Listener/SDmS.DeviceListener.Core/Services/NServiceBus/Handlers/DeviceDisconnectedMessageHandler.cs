using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceDisconnectedMessageHandler : IHandleMessages<DeviceDisconnectEvent>
    {
        private readonly ILog _logger;
        private readonly IDeviceManager _deviceManager;

        public DeviceDisconnectedMessageHandler(IDeviceManager deviceManager)
        {
            _logger = LogManager.GetLogger<DeviceDisconnectedMessageHandler>();
            this._deviceManager = deviceManager;
        }

        public async Task Handle(DeviceDisconnectEvent message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceDisconnectEvent recived. DETAILS:\n{message.ToString()}");

            var result = await this._deviceManager.ChangeOneDeviceParameterAsync(message.serial_number, "is_online", false);

            _logger.Info($"DeviceDisconnectEvent result. DETAILS:\nModified count: {result.ModifiedCount}");
        }
    }
}
