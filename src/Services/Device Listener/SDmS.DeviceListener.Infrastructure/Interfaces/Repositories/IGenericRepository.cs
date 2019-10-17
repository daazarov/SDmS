using MongoDB.Driver;
using SDmS.DeviceListener.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SDmS.DeviceListener.Infrastructure.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : MongoEntity
    {
        Task<T> GetAsync(string Id);
        Task<T> GetBySerialNumberAsync(string serial_number);
        Task CreateAsync(T device);
        Task<DeleteResult> DeleteAsync(string id);
        Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter);
        Task<UpdateResult> UpdateAsync(T device, UpdateDefinition<T> update);
        Task<ReplaceOneResult> ReplaceAsync(T device);
    }
}
