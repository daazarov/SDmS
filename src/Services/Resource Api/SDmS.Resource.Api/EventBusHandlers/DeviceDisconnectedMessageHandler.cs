using NServiceBus;
using NServiceBus.Logging;
using SDmS.Messages.Common.Events;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceDisconnectedMessageHandler : IHandleMessages<DeviceDisconnectEvent>
    {
        private readonly ILog _logger;
        private readonly IDeviceService _deviceService;
        private readonly IErrorInformatorService _errorInformatorService;

        public DeviceDisconnectedMessageHandler(IDeviceService deviceService, IErrorInformatorService errorInformatorService)
        {
            _logger = LogManager.GetLogger<DeviceDisconnectedMessageHandler>();
            this._deviceService = deviceService;
            this._errorInformatorService = errorInformatorService;
        }

        public async Task Handle(DeviceDisconnectEvent message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceDisconnectEvent recived. DETAILS:\n{message.ToString()}");

            var device = await _deviceService.GetDeviceAsync(message.serial_number);

            if (device != null)
            {
                device.is_online = false;

                await _deviceService.UpdateDeviceAsync(device);
            }
        }
    }
}
