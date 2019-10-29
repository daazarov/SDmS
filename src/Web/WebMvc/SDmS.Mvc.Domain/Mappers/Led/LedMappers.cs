using SDmS.Domain.Core.Models.Led;
using SDmS.Infrastructure.Models.Devices.Led;

namespace SDmS.Domain.Mappers.Led
{
    public static class LedMappers
    {
        public static LedDomainModel InfrastructureToDomain(this LedInfrastructureModel @this)
        {
            return new LedDomainModel
            {
                device_id = @this.device_id,
                is_enabled = @this.parameters.is_enabled,
                is_online = @this.is_online,
                name = @this.name,
                power = @this.parameters.power,
                serial_number = @this.serial_number,
                user_id = @this.user_id,
                intensity = @this.parameters.intensity,
                min_voltage = @this.parameters.min_voltage,
                max_voltage = @this.parameters.max_voltage
            };
        }
    }
}
