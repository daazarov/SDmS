using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Interfaces.Services
{
    public interface IDeviceManager
    {
        Task<UpdateResult> ChangeOneDeviceParameterAsync<T>(string serialNumber, string collection, string parameterName, T value);
        Task<UpdateResult> ChangeOneDeviceParameterAsync<T>(string serialNumber, string parameterName, T value);
        Task<DeleteResult> DeleteOneAsync(string collection, string serialNumber);
    }
}
