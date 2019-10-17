using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SDmS.DeviceListener.Infrastructure.Data.Entities;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using SDmS.DeviceListener.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Infrastructure.Data.Repositories
{
    public class TempSensorRepository : ITempSensorRepository
    {
        private readonly IDeviceListenerContext _context;
        private readonly ILogger _logger;

        public TempSensorRepository(IDeviceListenerContext context, ILogger<TempSensorRepository> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAsync(TempSensor device)
        {
            await _context.TempSensors.InsertOneAsync(device);
            _logger.LogInformation($"Created device document: s/n {device.serial_number}");
        }

        public async Task<DeleteResult> DeleteAsync(string id)
        {
            var result = await _context.TempSensors.DeleteOneAsync(x => x.id == id);

            if (result.IsAcknowledged) _logger.LogInformation($"Removed device document: _id {id}");
            else _logger.LogWarning($"Failed to remove device document: _id {id}");

            return result;
        }

        public async Task<IEnumerable<TempSensor>> FindAsync(System.Linq.Expressions.Expression<Func<TempSensor, bool>> filter)
        {
            return await _context.TempSensors.Find(filter).ToListAsync();
        }

        public async Task<TempSensor> GetAsync(string Id)
        {
            return await _context.TempSensors.FindAsync(x => x.id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<TempSensor> GetBySerialNumberAsync(string serial_number)
        {
            return await _context.TempSensors.FindAsync(x => x.serial_number == serial_number).Result.FirstOrDefaultAsync();
        }

        public async Task<ReplaceOneResult> ReplaceAsync(TempSensor device)
        {
            var result = await _context.TempSensors.ReplaceOneAsync(x => x.id == device.id, device);

            if (result.IsAcknowledged) _logger.LogInformation($"Replaced device document: _id {device.id}");
            else _logger.LogWarning($"Failed to replace device document: _id {device.id}");

            return result;
        }

        public async Task<UpdateResult> UpdateAsync(TempSensor device, UpdateDefinition<TempSensor> update)
        {
            var result = await _context.TempSensors.UpdateOneAsync(x => x.id == device.id, update);

            if (result.IsAcknowledged) _logger.LogInformation($"Updated device document: _id {device.id}");
            else _logger.LogWarning($"Failed to update device document: _id {device.id}");

            return result;
        }
    }
}
