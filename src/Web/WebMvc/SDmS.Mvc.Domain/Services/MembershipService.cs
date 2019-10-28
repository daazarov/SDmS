using SDmS.Domain.Core.Interfases.Services;
using SDmS.Domain.Core.Models;
using SDmS.Domain.Core.Models.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using SDmS.Domain.Mappers.Account;
using System.Threading.Tasks;
using SDmS.Mvc.Infrastructure.Services;
using SDmS.Mvc.Infrastructure;
using SDmS.Mvc.Infrastructure.Models.Account;

namespace SDmS.Domain.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly string _baseAccountUrl;

        public MembershipService()
        {
            _baseAccountUrl = ConfigurationManager.AppSettings["as:IdentityUrl"];
        }

        public async Task<CommandResult<string>> ConfirmEmailAddresssAsync(string userId, string code)
        {
            string uri = API.Account.ConfirmEmail(_baseAccountUrl, userId, code);

            var result = await CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").RunAsync<string>(uri);

            if (result != null)
            {
                CommandResult<string> commandResult = new CommandResult<string>();
                commandResult.Code = (int)result.ResponseCode;
                commandResult.Value = result.Value;
                commandResult.Error = result.Error;
                commandResult.Response = result.Response;

                return commandResult;
            }

            return null;
        }

        public async Task ForgotPasswordAsync(string email, string callbackUrl)
        {
            string uri = API.Account.ForgotPassword(_baseAccountUrl);

            await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND").RunAsync(uri, new ForgotPasswordModel { Email = email, CallbackUrl = callbackUrl});
        }

        public async Task<CommandResult<AccountCreatedResponseModel>> RegistrationAsync(Core.Models.AccountRegistrationModel model)
        {
            string uri = API.Account.Registration(_baseAccountUrl);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND").RunAsync<AccountCreatedModel>(uri, model.DomainToInfrastructure());

            if (result != null)
            {
                CommandResult<AccountCreatedResponseModel> commandResult = new CommandResult<AccountCreatedResponseModel>();
                commandResult.Code = (int)result.ResponseCode;
                commandResult.Value = result.Value?.InfrastructureToDomain();
                commandResult.Error = result.Error;
                commandResult.Response = result.Response;

                return commandResult;
            }

            return null;
        }

        public async Task<CommandResult<string>> ResetPasswordAsync(Core.Models.AccountResetPasswordModel model)
        {
            string uri = API.Account.ResetPassword(_baseAccountUrl);

            var result = await CommandFactory.Instance.GetCommand("BASE_POST_COMMAND").RunAsync<string>(uri, model.DomainToInfrastructure());

            if (result != null)
            {
                CommandResult<string> commandResult = new CommandResult<string>();
                commandResult.Code = (int)result.ResponseCode;
                commandResult.Value = result.Value;
                commandResult.Error = result.Error;
                commandResult.Response = result.Response;

                return commandResult;
            }

            return null;
        }

        public async Task<CommandResult<JwtTokenModel>> SingUpAsync(Core.Models.AccountLoginModel model)
        {
            var result = await CommandFactory.Instance.GetCommand("LOGIN").RunAsync<TokenModel>(model.DomainToInfrastructure());

            if (result != null)
            {
                CommandResult<JwtTokenModel> commandResult = new CommandResult<JwtTokenModel>();
                commandResult.Code = (int)result.ResponseCode;
                commandResult.Value = result.Value?.InfrastructureToDomain();
                commandResult.Error = result.Error;
                commandResult.Response = result.Response;

                return commandResult;
            }

            return null;
        }
    }
}
