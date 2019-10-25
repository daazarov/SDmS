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
                id = @this.id,
                is_enabled = @this.is_enabled,
                is_online = @this.is_online,
                name = @this.name,
                power = @this.power,
                serial_number = @this.serial_number,
                user_id = @this.user_id,
                intensity = @this.intensity,
                min_voltage = @this.min_voltage,
                max_voltage = @this.max_voltage
            };
        }
    }
}
