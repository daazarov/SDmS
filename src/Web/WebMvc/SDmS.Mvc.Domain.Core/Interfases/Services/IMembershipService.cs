using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Account;
using System.Threading.Tasks;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface IMembershipService
    {
        Task<CommandResult<JwtTokenModel>> SingUpAsync(AccountLoginModel model);
        Task<CommandResult<AccountCreatedResponseModel>> RegistrationAsync(AccountRegistrationModel model);
        Task<CommandResult<string>> ConfirmEmailAddresssAsync(string userId, string code);
        Task ForgotPasswordAsync(string email, string callbackUrl);
        Task<CommandResult<string>> ResetPasswordAsync(AccountResetPasswordModel model);
    }
}
