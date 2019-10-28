using SDmS.Domain.Core.Models.Climate;
using SDmS.Mvc.Areas.Dashboard.Models.Climate;

namespace SDmS.Mvc.Areas.Dashboard.Mappers.Climate
{
    public static class TempControleMapper
    {
        public static TempControlViewModel DomainToView(this TempControlDomainModel @this)
        {
            return new TempControlViewModel
            {
                Name = @this.name,
                SerialNumber = @this.serial_number,
                DesiredTemp = @this.desired_temp,
                IsControlEnable = @this.is_control_enable,
                IsOnline = @this.is_online,
                TempC = @this.temp_c
            };
        }
    }
}