using SDmS.Resource.Domain.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace SDmS.Resource.Domain.Services
{
    public class ErrorInformatorService : IErrorInformatorService
    {
        public string GetDescription(int error_code)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDescriptionAsync(int error_code)
        {
            throw new NotImplementedException();
        }
    }
}
