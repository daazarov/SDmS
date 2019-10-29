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
                desired_temp = @this.parameters.desired_temp,
                is_control_enable = @this.parameters.is_control_enable,
                is_online = @this.is_online,
                temp_c = @this.parameters.temp_c
            };
        }
    }
}
