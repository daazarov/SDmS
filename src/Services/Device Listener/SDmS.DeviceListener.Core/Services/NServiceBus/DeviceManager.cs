using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Core.Services.NServiceBus
{
    public class DeviceManager : IDeviceManager
    {
        private readonly ILogger _logger;
        private readonly IDeviceListenerContext _context;

        public DeviceManager(ILogger<DeviceManager> logger, IDeviceListenerContext context)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<UpdateResult> ChangeManyDeviceParameterAsync(string collection)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateResult> ChangeOneDeviceParameterAsync<T>(string serialNumber, string collectionName, string parameterName, T value)
        {
            var collection = _context.Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("serial_number", serialNumber);

            var devices = await collection.FindAsync(filter);
            var device = await devices.FirstOrDefaultAsync();

            if (device != null)
            {
                if (!(device[parameterName] is T)) throw new InvalidOperationException();
            }
            //else throw new InvalidOperationException("Error. Device with serial number {serialNumber} was not found");

            var update = Builders<BsonDocument>.Update.Set("parameterName", value).CurrentDate("last_modified");
            var result = await collection.UpdateOneAsync(filter, update);

            return result;
        }
    }
}
