using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Resource.Domain.Interfaces.Services
{
    public interface IDeviceService
    {
        void AddDevice();
        void DeleteDevice();
        void ExecuteAction();

    }
}
