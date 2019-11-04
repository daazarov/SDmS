using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Commands;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class DeviceAssignCommandHandler : IHandleMessages<DeviceAssignCommand>
    {
        private readonly ILog _logger;
        private readonly INewDeviceService _newDeviceService;

        public DeviceAssignCommandHandler(INewDeviceService newDeviceService)
        {
            _logger = LogManager.GetLogger<DeviceAssignCommandHandler>();
            this._newDeviceService = newDeviceService;
        }

        public async Task Handle(DeviceAssignCommand message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceAssignCommand recived. DETAILS:\n{message.ToString()}");

            var result = await this._newDeviceService.AssignToUserAsync(message, message.type_text);

            if (!result)
            {
                throw new InvalidOperationException("Error adding device. View the log files for details");
            }
        }
    }
}
