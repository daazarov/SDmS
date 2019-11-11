using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceConnectedMessageHandler : IHandleMessages<DeviceConnectEvent>
    {
        private readonly ILog _logger;
        private readonly IDeviceManager _deviceManager;

        public DeviceConnectedMessageHandler(IDeviceManager deviceManager)
        {
            _logger = LogManager.GetLogger<DeviceDisconnectedMessageHandler>();
            this._deviceManager = deviceManager;
        }

        public async Task Handle(DeviceConnectEvent message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceConnectEvent recived. DETAILS:\n{message.ToString()}");

            var result = await this._deviceManager.ChangeOneDeviceParameterAsync(message.serial_number, "is_online", true);

            _logger.Info($"DeviceConnectEvent result. DETAILS:\nModified count: {result.ModifiedCount}");
        }
    }
}
