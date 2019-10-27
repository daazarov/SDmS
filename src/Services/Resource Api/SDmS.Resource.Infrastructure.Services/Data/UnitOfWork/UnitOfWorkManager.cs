using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Services.Data.Context;

namespace SDmS.Resource.Infrastructure.Services.Data
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly ResourceDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkManager(ResourceDbContext context, IUnitOfWork unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
            //return new UnitOfWork(_context);
        }
    }
}
