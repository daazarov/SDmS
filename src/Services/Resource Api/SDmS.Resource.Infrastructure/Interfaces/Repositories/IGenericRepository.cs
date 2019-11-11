using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SDmS.Resource.Infrastructure.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// return the queryable entity set for the given type {T}.
        /// </summary>
        IQueryable<T> Table { get; }
        /// <summary>
        /// Find entity by id
        /// </summary>
        /// <param name="id">id of entity. This can also be a composite key</param>
        /// <returns>The resolved entity</returns>
        T FindById(object id);
        Task<T> FindByIdAsync(object id);
        IEnumerable<T> GetAll(int limit, int offset);
        Task<IEnumerable<T>> GetAllAsync(int limit, int offset);
        IEnumerable<T> GetbyFilter(Expression<Func<T, bool>> filter, int limit = 0, int offset = 0);
        Task<IEnumerable<T>> GetbyFilterAsync(Expression<Func<T, bool>> filter, int limit = 0, int offset = 0);
        void Insert(T entity);
        Task InsertAsync(T entity);
        void InsertRange(IEnumerable<T> entities);
        Task InsertRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        bool IsAutoCommitEnable { get; set; }
    }
}
