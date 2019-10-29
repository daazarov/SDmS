using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SDmS.Infrastructure.Interfaces;
using SDmS.Infrastructure.Commands;
using SDmS.Mvc.Infrastructure.Models.Account;
using SDmS.Infrastructure.Models;
using System.Configuration;

namespace SDmS.Infrastructure.Command.Account
{
    public class LoginCommand : PostCommand
    {
        private const string NAME = "LOGIN";
        private static string URI = $"{ConfigurationManager.AppSettings["as:IdentityUri"]}/oauth/token";

        public LoginCommand() : base(NAME, URI)
        {
            this.UseToken = false;
        }

        protected override async Task<ICommandResult<T>> ExecuteAsync<T>(object model)
        {
            if (model != null && model is AccountLoginModel)
            {
                AccountLoginModel loginModel = model as AccountLoginModel;

                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", loginModel.Username),
                    new KeyValuePair<string, string>("password", loginModel.Password),
                    new KeyValuePair<string, string>("remember", loginModel.RememberMe.ToString()),
                };

                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await _client.PostAsync(URI, requestParamsFormUrlEncoded);

                return await GetResultAsync<T>(tokenServiceResponse);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}