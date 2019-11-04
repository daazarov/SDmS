using SDmS.Resource.Api.Models.Devices;
using SDmS.Resource.Domain.Models.Devices;
using System.Collections.Generic;

namespace SDmS.Resource.Api.Models.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceRequestDomainModel RequestToDomain(this DeviceRequestModel @this)
        {
            return new DeviceRequestDomainModel
            {
                limit = @this.limit ?? 0,
                offset = @this.offset ?? 0,
                type = @this.type
            };
        }

        public static DeviceResponseModel DomainToResponse(this DeviceDomainModel @this)
        {
            DeviceResponseModel model = new DeviceResponseModel
            {
                device_id = @this.device_id,
                name = @this.name,
                serial_number = @this.serial_number,
                user_id = @this.user_id,
                is_online = @this.is_online,
                parameters = new Dictionary<string, dynamic>()
            };

            foreach (var parameter in @this.DeviceParameters)
            {
                model.parameters.Add(parameter.parameter_name, parameter.value);
            }

            return model;
        }

        public static DeviceAddDomainModel RequestToDomain(this DeviceAddModel @this)
        {
            return new DeviceAddDomainModel
            {
                name = @this.name,
                serial_number = @this.serial_number,
                type = @this.type
            };
        }
    }
}
