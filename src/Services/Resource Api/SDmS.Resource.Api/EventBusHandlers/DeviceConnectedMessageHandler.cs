using NServiceBus;
using NServiceBus.Logging;
using SDmS.Messages.Common.Events;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceConnectedMessageHandler : IHandleMessages<DeviceConnectEvent>
    {
        private readonly ILog _logger;
        private readonly IDeviceService _deviceService;
        private readonly IErrorInformatorService _errorInformatorService;

        public DeviceConnectedMessageHandler(IDeviceService deviceService, IErrorInformatorService errorInformatorService)
        {
            _logger = LogManager.GetLogger<DeviceConnectedMessageHandler>();
            this._deviceService = deviceService;
            this._errorInformatorService = errorInformatorService;
        }

        public async Task Handle(DeviceConnectEvent message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceConnectEvent recived. DETAILS:\n{message.ToString()}");

            var device = await _deviceService.GetDeviceAsync(message.serial_number);

            if (device != null)
            {
                device.is_online = true;

                await _deviceService.UpdateDeviceAsync(device);
            }
        }
    }
}
