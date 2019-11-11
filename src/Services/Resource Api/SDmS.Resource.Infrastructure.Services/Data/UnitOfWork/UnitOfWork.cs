using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Services.Data.Context;
using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using SDmS.Resource.Infrastructure.Services.Data.Repositories;

namespace SDmS.Resource.Infrastructure.Services.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ResourceDbContext _context;
        private Dictionary<string, object> repositories;

        public DbContext DataBase => _context;

        public UnitOfWork(ResourceDbContext context)
        {
            this._context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
            //_transaction.Commit();
        }

        public void Rollback()
        {
            //_transaction.Rollback();

            // http://blog.oneunicorn.com/2011/04/03/rejecting-changes-to-entities-in-ef-4-1/

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        // Note - problem with deleted entities:
                        // When an entity is deleted its relationships to other entities are severed. 
                        // This includes setting FKs to null for nullable FKs or marking the FKs as conceptually null (don’t ask!) 
                        // if the FK property is not nullable. You’ll need to reset the FK property values to 
                        // the values that they had previously in order to re-form the relationships. 
                        // This may include FK properties in other entities for relationships where the 
                        // deleted entity is the principal of the relationship–e.g. has the PK 
                        // rather than the FK. I know this is a pain–it would be great if it could be made easier in the future, but for now it is what it is.
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<T>)repositories[type];
        }

        public TRepo Repository<TRepo, DbEntity>() 
            where TRepo : IGenericRepository<DbEntity>
            where DbEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(DbEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(DbEntity)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (TRepo)repositories[type];
        }
    }
}
