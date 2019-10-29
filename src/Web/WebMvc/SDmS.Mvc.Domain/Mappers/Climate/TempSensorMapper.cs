using SDmS.Domain.Core.Models.Climate;
using SDmS.Infrastructure.Models.Devices.Climate;

namespace SDmS.Domain.Mappers.Climate
{
    public static class TempSensorMapper
    {
        public static TempSensorDomainModel InfrastructureToDomain(this TempSensorInfrastructureModel @this)
        {
            return new TempSensorDomainModel
            {
                device_id = @this.device_id,
                user_id = @this.user_id,
                name = @this.name,
                serial_number = @this.serial_number,
                is_online = @this.is_online,
                temp_c = @this.parameters.temp_c
            };
        }
    }
}
