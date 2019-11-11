using SDmS.Domain.Core.Models.Climate;
using SDmS.Infrastructure.Models.Devices.Climate;

namespace SDmS.Domain.Mappers.Climate
{
    public static class TempControlMapper
    {
        public static TempControlDomainModel InfrastructureToDomain(this TempControlInfrastructureModel @this)
        {
            return new TempControlDomainModel
            {
                device_id = @this.device_id,
                user_id = @this.user_id,
                serial_number = @this.serial_number,
                name = @this.name,
                desired_temp = @this.parameters.TryGetValue("desired_temperature", out dynamic val_desired_temperature) ? int.Parse(val_desired_temperature) : 24,
                is_control_enable = @this.parameters.TryGetValue("desired_enabled", out dynamic val_desired_enabled) ? bool.Parse(val_desired_enabled) : false,
                is_online = @this.is_online,
                temp_c = @this.parameters.TryGetValue("temperature_data", out dynamic val_temperature_data) ? double.Parse(val_temperature_data) : -127
            };
        }
    }
}
