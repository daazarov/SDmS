using Microsoft.EntityFrameworkCore;
using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using SDmS.Resource.Infrastructure.Services.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SDmS.Resource.Infrastructure.Services.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ResourceDbContext _context;
        protected DbSet<T> _entities;
        protected bool _autoCommit = false;

        protected DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities as DbSet<T>;
            }
        }

        public GenericRepository(ResourceDbContext context)
        {
            this._context = context as ResourceDbContext;
        }

        #region interface members
        public bool IsAutoCommitEnable
        {
            get
            {
                return _autoCommit;
            }

            set
            {
                this._autoCommit = value;
            }
        }

        public IQueryable<T> Table
        {
            get
            {
                return (IQueryable<T>)this.Entities;
            }
        }

        public void Delete(T entity)
        {
            this.Entities.Remove(entity);
            Commit();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                this.Entities.Remove(entity);
            }
            Commit();
        }

        public T FindById(object id)
        {
            return this.Entities.Find(id);
        }

        public async Task<T> FindByIdAsync(object id)
        {
            return await this.Entities.FindAsync(id);
        }

        public IEnumerable<T> GetAll(int limit, int offset)
        {
            return this.Entities.Skip(offset).Take(limit).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int limit, int offset)
        {
            return await this.Entities.Skip(offset).Take(limit).ToListAsync();
        }

        public IEnumerable<T> GetbyFilter(Expression<Func<T, bool>> filter)
        {
            return this.Entities.Where(filter).ToList();
        }

        public async Task<IEnumerable<T>> GetbyFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await this.Entities.Where(filter).ToListAsync();
        }

        public void Insert(T entity)
        {
            this.Entities.Add(entity);
            Commit();
        }

        public async Task InsertAsync(T entity)
        {
            await this.Entities.AddAsync(entity);
            await CommitAsync();
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            this.Entities.AddRange(entities);
            Commit();
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await this.Entities.AddRangeAsync(entities);
            await CommitAsync();
        }

        public void Update(T entity)
        {
            ChangeStateToModifiedIfApplicable(entity);
            Commit();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                ChangeStateToModifiedIfApplicable(entity);
            }
            Commit();
        }
        #endregion

        #region Helping methods
        private void Commit()
        {
            if (_autoCommit)
                _context.SaveChanges();
        }

        private async Task CommitAsync()
        {
            if (_autoCommit)
                await _context.SaveChangesAsync();
        }

        private void ChangeStateToModifiedIfApplicable(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                // Entity was detached before or was explicitly constructed.
                // This unfortunately sets all properties to modified.
                entry.State = EntityState.Modified;
            }
            else if (entry.State == EntityState.Unchanged)
            {
                // We simply do nothing here, because it is ensured now that DetectChanges()
                // gets implicitly called prior SaveChanges().
            }
        }
        #endregion
    }
}
