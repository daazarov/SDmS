using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using System;

namespace SDmS.Resource.Infrastructure.Interfaces
{
    public interface IUnitOfWork //: IDisposable
    {
        TRepo Repository<TRepo, DbEntity>() 
            where TRepo : IGenericRepository<DbEntity>
            where DbEntity : class;

        void Commit();
        void Rollback();

    }
}
