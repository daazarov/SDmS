using SDmS.Mvc.Areas.Dashboard.Models.Led;
using System.Collections.Generic;

namespace SDmS.Mvc.Areas.Dashboard.Models.Pages
{
    public class LightPageViewModel
    {
        public IEnumerable<LedViewModel> Leds { get; set; }
        public AddDeviceViewModel LedAdd { get; set; }
    }
}