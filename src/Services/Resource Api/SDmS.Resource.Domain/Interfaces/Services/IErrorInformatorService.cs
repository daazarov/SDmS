using System.Threading.Tasks;

namespace SDmS.Resource.Domain.Interfaces.Services
{
    public interface IErrorInformatorService
    {
        string GetDescription(int error_code);
        Task<string> GetDescriptionAsync(int error_code);
    }
}
