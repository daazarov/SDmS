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
                serial_number = @this.serial_number,
                name = @this.name,
                desired_temp = @this.desired_temp,
                is_control_enable = @this.is_control_enable,
                is_online = @this.is_online,
                temp_c = @this.temp_c
            };
        }
    }
}
