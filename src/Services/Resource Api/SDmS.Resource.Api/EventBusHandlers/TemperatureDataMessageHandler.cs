using NServiceBus;
using NServiceBus.Logging;
using SDmS.Messages.Common.Events;
using SDmS.Messages.Common.Messages;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class TemperatureDataMessageHandler : IHandleMessages<TemperatureDataEvent>
    {
        private readonly ILog _logger;
        private readonly IDeviceService _deviceService;

        public TemperatureDataMessageHandler(IDeviceService deviceService)
        {
            _logger = LogManager.GetLogger<TemperatureDataMessageHandler>();
            this._deviceService = deviceService;
        }

        public async Task Handle(TemperatureDataEvent message, IMessageHandlerContext context)
        {
            _logger.Info($"TemperatureDataMessage recived. DETAILS:\n{message.ToString()}");

            if (Double.TryParse(message.temperature_data, NumberStyles.Any, CultureInfo.InvariantCulture, out double data))
            {
                try
                {
                    var result = await _deviceService.UpdateDeviceParameterAsync(message.serial_number, "TEMPERATURE_DATA", message.temperature_data);
                }
                catch (ResourceException e)
                {
                    if (e.ErrorCode != -201)
                    {
                        throw new ResourceException(e.Message, e.ErrorCode, e.HttpResponseCode);
                    }
                }
            }
            else throw new InvalidCastException($"Failed to convert temperature data to double type: {message.temperature_data}");
        }
    }
}
