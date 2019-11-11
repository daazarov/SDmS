using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Commands;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceDeleteCommandHandler : IHandleMessages<DeviceDeleteCommand>
    {
        private readonly ILog _logger;
        private readonly IDeviceManager _deviceManager;

        public DeviceDeleteCommandHandler(IDeviceManager deviceManager)
        {
            _logger = LogManager.GetLogger<TemperatureDataMessageHandler>();
            this._deviceManager = deviceManager;
        }

        public async Task Handle(DeviceDeleteCommand message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceDeleteCommand recived. DETAILS:\n{message.ToString()}");

            var result = await _deviceManager.DeleteOneAsync(message.type_text, message.serial_number);

            _logger.Info($"DeviceDeleteCommand result. DETAILS:\nDeleted count: {result.DeletedCount}");
        }
    }
}
