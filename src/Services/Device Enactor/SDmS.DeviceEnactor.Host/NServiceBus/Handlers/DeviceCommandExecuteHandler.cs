using NServiceBus;
using NServiceBus.Logging;
using SDmS.Messages.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceEnactor.Host.NServiceBus.Handlers
{
    public class DeviceCommandExecuteHandler : IHandleMessages<DeviceCommandExecute>
    {
        private readonly ILog _logger;

        public DeviceCommandExecuteHandler()
        {
            _logger = LogManager.GetLogger<DeviceCommandExecuteHandler>();
        }

        public async Task Handle(DeviceCommandExecute message, IMessageHandlerContext context)
        {
            _logger.Info($"DeviceCommandExecute recived. DETAILS:\n{message.ToString()}");
        }
    }
}
