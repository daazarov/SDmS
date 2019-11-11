using Microsoft.EntityFrameworkCore;
using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using System;

namespace SDmS.Resource.Infrastructure.Interfaces
{
    public interface IUnitOfWork //: IDisposable
    {
        TRepo Repository<TRepo, DbEntity>() 
            where TRepo : IGenericRepository<DbEntity>
            where DbEntity : class;

        DbContext DataBase { get; }

        void Commit();
        void Rollback();

    }
}
