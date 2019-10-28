using SDmS.Domain.Core.Models.Climate;
using SDmS.Mvc.Areas.Dashboard.Models.Climate;

namespace SDmS.Mvc.Areas.Dashboard.Mappers.Climate
{
    public static class TempSensorMapper
    {
        public static TempSensorViewModel DomainToView(this TempSensorDomainModel @this)
        {
            return new TempSensorViewModel
            {
                Name = @this.name,
                IsOnline = @this.is_online,
                SerialNumber = @this.serial_number,
                TempC = @this.temp_c
            };
        }
    }
}