using Microsoft.Extensions.Logging;
using SDmS.Resource.Common.Entities;
using SDmS.Resource.Domain.Interfaces.Services;
using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Interfaces.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace SDmS.Resource.Domain.Services
{
    public class ErrorInformatorService : IErrorInformatorService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger _logger;

        public ErrorInformatorService(IUnitOfWorkManager unitOfWorkManager, ILogger<ErrorInformatorService> logger)
        {
            this._logger = logger;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public string GetDescription(int error_code)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var repository = unitOfWork.Repository<IGenericRepository<ErrorDescription>, ErrorDescription>();

            var error = repository.GetbyFilter(x => x.error_code == error_code, 0, 0).FirstOrDefault();

            return (error != null)
                ? error.description
                : "Unidentified error";
        }

        public async Task<string> GetDescriptionAsync(int error_code)
        {
            var unitOfWork = _unitOfWorkManager.GetUnitOfWork();

            var repository = unitOfWork.Repository<IGenericRepository<ErrorDescription>, ErrorDescription>();

            var error = await repository.GetbyFilterAsync(x => x.error_code == error_code, 0, 0);

            return (error.FirstOrDefault() != null)
                ? error.FirstOrDefault().description
                : "Unidentified error";
        }
    }
}
