using SDmS.Domain.Core.Models.Account;
using SDmS.Mvc.Infrastructure.Models.Account;
using System;

namespace SDmS.Domain.Mappers.Account
{
    public static class AccountMappers
    {
        public static AccountCreatedResponseModel InfrastructureToDomain(this AccountCreatedModel @this)
        {
            return new AccountCreatedResponseModel
            {
                Email = @this.Email,
                FirstName = @this.FirstName,
                Id = @this.Id,
                LastName = @this.LastName,
                Username = @this.Username
            };
        }

        public static AccountLoginModel DomainToInfrastructure(this Core.Models.AccountLoginModel @this)
        {
            return new AccountLoginModel
            {
                Username = @this.Username,
                Password = @this.Password,
                RememberMe = @this.RememberMe
            };
        }

        public static JwtTokenModel InfrastructureToDomain(this TokenModel @this)
        {
            return new JwtTokenModel
            {
                Token = @this.access_token,
                TokenType = @this.token_type,
                Expires = DateTime.UtcNow.AddSeconds(@this.expires_in)
            };
        }

        public static AccountRegistrationModel DomainToInfrastructure(this Core.Models.AccountRegistrationModel @this)
        {
            return new AccountRegistrationModel
            {
                ConfirmCallbackUrl = @this.ConfirmCallbackUrl,
                Email = @this.Email,
                FirstName = @this.FirstName,
                LastName = @this.LastName,
                Password = @this.Password,
                Username = @this.Username
            };
        }

        public static AccountResetPasswordModel DomainToInfrastructure(this Core.Models.AccountResetPasswordModel @this)
        {
            return new AccountResetPasswordModel
            {
                Code = @this.Code,
                Email = @this.Email,
                Password = @this.Password
            };
        }
    }
}
