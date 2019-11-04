using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Messages;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceVerificationMessageHandler : IHandleMessages<DeviceVerificationMessage>
    {
        private readonly ILog _logger;
        private readonly INewDeviceService _newDeviceService;

        public DeviceVerificationMessageHandler(INewDeviceService newDeviceService)
        {
            _logger = LogManager.GetLogger<DeviceVerificationMessageHandler>();
            this._newDeviceService = newDeviceService;
        }

        public async Task Handle(DeviceVerificationMessage message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceVerificationMessage recived. DETAILS:\n{message.ToString()}");

            if (message.CheckType.ToUpper() == "EXISTENCE_IN_NEW")
            {
                var response = await this._newDeviceService.CheckDeviceExistenceInNewAsync(message.serial_number);
                await context.Reply(response).ConfigureAwait(false);
            }
            else
            {
                throw new InvalidOperationException($"Unknown check type: {message.CheckType}");
            }
        }
    }
}
