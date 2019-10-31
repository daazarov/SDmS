using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Events;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class BaseHelloMessageHandler : IHandleMessages<DeviceHelloEvent>
    {
        private readonly INewDeviceService _newDeviceService;
        private readonly ILog _logger;

        public BaseHelloMessageHandler(INewDeviceService newDeviceService)
        {
            this._newDeviceService = newDeviceService ?? throw new ArgumentNullException(nameof(newDeviceService));
            _logger = LogManager.GetLogger<BaseHelloMessageHandler>();
        }

        public async Task Handle(DeviceHelloEvent message, IMessageHandlerContext context)
        {
            await _newDeviceService.RegisterDeviceAsync<DeviceHelloEvent>(message);
        }
    }
}
