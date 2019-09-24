using SDmS.Domain.Core.Models;
using SDmS.Mvc.Models.Account;

namespace SDmS.Mvc.Mappers
{
    public static class AccountMappers
    {
        public static AccountLoginModel ViewToDomain(this AccountLoginViewModel @this)
        {
            return new AccountLoginModel
            {
                Username = @this.Username,
                Password = @this.Password,
                RememberMe = @this.RememberMe
            };
        }

        public static AccountRegistrationModel ViewToDomain(this AccountRegistrationViewModel @this)
        {
            return new AccountRegistrationModel
            {
                Email = @this.Email,
                FirstName = @this.FirstName,
                LastName = @this.LastName,
                Password = @this.Password,
                Username = @this.Username,
                ConfirmCallbackUrl = @this.ConfirmCallbackUrl
            };
        }
    }
}