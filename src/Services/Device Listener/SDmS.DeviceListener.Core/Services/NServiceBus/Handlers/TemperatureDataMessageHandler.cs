using NServiceBus;
using NServiceBus.Logging;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.Messages.Common.Messages;
using System;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus.Handlers
{
    public class TemperatureDataMessageHandler : IHandleMessages<TemperatureDataMessage>
    {
        private readonly ILog _logger;
        private readonly IDeviceManager _deviceManager;

        public TemperatureDataMessageHandler(IDeviceManager deviceManager)
        {
            _logger = LogManager.GetLogger<TemperatureDataMessageHandler>();
            this._deviceManager = deviceManager;
        }

        public async Task Handle(TemperatureDataMessage message, IMessageHandlerContext context)
        {
            _logger.Info($"TemperatureDataMessage recived. DETAILS:\n{message.ToString()}");

            if (Double.TryParse(message.temperature_data, out double data))
            {
                var result = await _deviceManager.ChangeOneDeviceParameterAsync(message.serial_number, message.type, "temperature_data", data);

                if (result.ModifiedCount == 0)
                {
                    throw new Exception("Failed to update temperature sendor data");
                }
            }
            else throw new InvalidCastException($"Failed to convert temperature data to double type: {message.temperature_data}");
        }
    }
}
