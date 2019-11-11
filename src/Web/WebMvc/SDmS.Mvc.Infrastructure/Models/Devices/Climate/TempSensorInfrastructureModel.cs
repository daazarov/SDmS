using System.Collections.Generic;

namespace SDmS.Infrastructure.Models.Devices.Climate
{
    public class TempSensorInfrastructureModel : DeviceInfrastructureModel
    {
        public Dictionary<string, dynamic> parameters { get; set; }
    }
}
