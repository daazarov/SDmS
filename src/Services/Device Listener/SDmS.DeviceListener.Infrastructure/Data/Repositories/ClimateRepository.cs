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
    public class ClimateRepository : IClimateRepository
    {
        private readonly IDeviceListenerContext _context;
        private readonly ILogger _logger;

        public ClimateRepository(IDeviceListenerContext context, ILogger<ClimateRepository> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAsync(Climate device)
        {
            await _context.ClimateDevices.InsertOneAsync(device);
            _logger.LogInformation($"Created device document: s/n {device.serial_number}");
        }

        public async Task<DeleteResult> DeleteAsync(string id)
        {
            var result = await _context.ClimateDevices.DeleteOneAsync(x => x.id == id);

            if (result.IsAcknowledged) _logger.LogInformation($"Removed device document: _id {id}");
            else _logger.LogWarning($"Failed to remove device document: _id {id}");

            return result;
        }

        public async Task<IEnumerable<Climate>> FindAsync(System.Linq.Expressions.Expression<Func<Climate, bool>> filter)
        {
            return await _context.ClimateDevices.Find(filter).ToListAsync();
        }

        public async Task<Climate> GetAsync(string Id)
        {
            return await _context.ClimateDevices.FindAsync(x => x.id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<Climate> GetBySerialNumberAsync(string serial_number)
        {
            return await _context.ClimateDevices.FindAsync(x => x.serial_number == serial_number).Result.FirstOrDefaultAsync();
        }

        public async Task<ReplaceOneResult> ReplaceAsync(Climate device)
        {
            var result = await _context.ClimateDevices.ReplaceOneAsync(x => x.id == device.id, device);

            if (result.IsAcknowledged) _logger.LogInformation($"Replaced device document: _id {device.id}");
            else _logger.LogWarning($"Failed to replace device document: _id {device.id}");

            return result;
        }

        public async Task<UpdateResult> UpdateAsync(Climate device, UpdateDefinition<Climate> update)
        {
            var result = await _context.ClimateDevices.UpdateOneAsync(x => x.id == device.id, update);

            if (result.IsAcknowledged) _logger.LogInformation($"Updated device document: _id {device.id}");
            else _logger.LogWarning($"Failed to update device document: _id {device.id}");

            return result;
        }
    }
}
