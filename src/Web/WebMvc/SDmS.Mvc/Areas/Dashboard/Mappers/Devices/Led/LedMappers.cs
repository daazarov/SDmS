using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Led;
using SDmS.Mvc.Areas.Dashboard.Models;
using SDmS.Mvc.Areas.Dashboard.Models.Led;

namespace SDmS.Mvc.Areas.Dashboard.Mappers.Led
{
    public static class LedMappers
    {
        public static LedViewModel DomainToView(this LedDomainModel @this)
        {
            var model = new LedViewModel();

            model.Name = @this.name;
            model.Power = @this.power;
            model.SerialNumber = @this.serial_number;
            model.IsOnline = @this.is_online;
            model.IsEnable = @this.is_enabled;
            model.Intensity = @this.intensity;

            if (@this.max_voltage > 0 && @this.min_voltage > 0)
            {
                model.VoltageRange = string.Format("{0} - {1}V", @this.min_voltage, @this.max_voltage);
            }
            else if (@this.max_voltage > 0 && @this.min_voltage == 0)
            {
                model.VoltageRange = string.Format("{0}V", @this.max_voltage);
            }
            else if (@this.max_voltage == 0 && @this.min_voltage > 0)
            {
                model.VoltageRange = string.Format("{0}V", @this.min_voltage);
            }
            else
            {
                model.VoltageRange = "no data";
            }

            return model;
        }
    }
}