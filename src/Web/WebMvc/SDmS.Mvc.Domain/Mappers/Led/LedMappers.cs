using SDmS.Domain.Core.Models.Led;
using SDmS.Infrastructure.Models.Devices.Led;
using System;

namespace SDmS.Domain.Mappers.Led
{
    public static class LedMappers
    {
        public static LedDomainModel InfrastructureToDomain(this LedInfrastructureModel @this)
        {
            return new LedDomainModel
            {
                device_id = @this.device_id,
                is_enabled = @this.parameters.TryGetValue("led_enabled", out dynamic valEnabled) ? bool.Parse(valEnabled) : false,
                is_online = @this.is_online,
                name = @this.name,
                power = @this.parameters.TryGetValue("led_power", out dynamic valPower) ? int.Parse(valPower) : 0,
                serial_number = @this.serial_number,
                user_id = @this.user_id,
                intensity = @this.parameters.TryGetValue("led_intensity", out dynamic valIntensity) ? int.Parse(valIntensity) : 0,
                min_voltage = @this.parameters.TryGetValue("led_min_voltage", out dynamic valMinVoltage) ? int.Parse(valMinVoltage) : 0,
                max_voltage = @this.parameters.TryGetValue("led_max_voltage", out dynamic maxMinVoltage) ? int.Parse(maxMinVoltage) : 0
            };
        }
    }
}
