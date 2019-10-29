using SDmS.Domain.Core.Models;
using SDmS.Mvc.Areas.Dashboard.Models;

namespace SDmS.Mvc.Areas.Dashboard.Mappers.Devices
{
    public static  class DeviceMappers
    {
        public static DeviceAddToUserDomainModel ViewToDomain(this AddDeviceViewModel @this)
        {
            return new DeviceAddToUserDomainModel
            {
                name = @this.Name,
                serial_number = @this.SerialNumber,
                user_id = @this.UserId,
                type = @this.Type
            };
        }
    }
}