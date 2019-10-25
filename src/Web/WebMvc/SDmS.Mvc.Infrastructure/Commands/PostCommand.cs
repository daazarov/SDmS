using Newtonsoft.Json;
using SDmS.Infrastructure.Attributes;
using SDmS.Infrastructure.Interfaces;
using SDmS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SDmS.Infrastructure.Commands
{
    public class PostCommand : ICommand
    {
        private const string NAME = "BASE_POST_COMMAND";

        private readonly string _commandName;
        private string _uri;
        protected readonly HttpClient _client;

        /// <summary>
        /// using JWT token, by default = true
        /// </summary>
        public bool UseToken { get; protected set; } = true;
        /// <summary>
        /// application/json
        /// </summary>
        public bool UseJson { get; protected set; } = true;

        public PostCommand(string commandName, string uri)
        {
            this._commandName = commandName;
            this._uri = uri;
            _client = new HttpClient();
        }
        public PostCommand() :this(NAME, null)
        { }

        #region Interface implimentation
        public string CommandName => _commandName;

        public async Task RunAsync(object properties)
        {
            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            await ExecuteAsync(properties);
        }

        public async Task RunAsync(string uri)
        {
            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            await ExecuteAsync(uri);
        }

        public async Task RunAsync(string uri, object properties)
        {

            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            await ExecuteAsync(uri, properties);
        }

        public async Task<ICommandResult<T>> RunAsync<T>(object properties)
        {
            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            ICommandResult<T> result = await ExecuteAsync<T>(properties);

            return result;
        }

        public async Task<ICommandResult<T>> RunAsync<T>(string uri)
        {
            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            ICommandResult<T> result = await ExecuteAsync<T>(uri);

            return result;
        }

        public async Task<ICommandResult<T>> RunAsync<T>(string uri, object properties)
        {
            if (UseToken)
                ApplyJWTToken();
            if (UseJson)
                ApplyJsonAccept();

            ICommandResult<T> result = await ExecuteAsync<T>(uri, properties);

            return result;
        }
        #endregion

        #region Abstract methods
        protected virtual async Task<ICommandResult<T>> ExecuteAsync<T>(object model)
        {
            if (string.IsNullOrEmpty(_uri))
            {
                throw new NullReferenceException(nameof(_uri));
            }

            return await ExecuteAsync<T>(_uri, model);
        }
        protected virtual async Task<ICommandResult<T>> ExecuteAsync<T>(string uri)
        {
            HttpResponseMessage response = await _client.PostAsync(uri, null);

            return await GetResultAsync<T>(response);
        }
        protected virtual async Task ExecuteAsync(object model)
        {
            if (string.IsNullOrEmpty(_uri))
            {
                throw new NullReferenceException(nameof(_uri));
            }

            await ExecuteAsync(_uri, model);
        }
        protected virtual async Task ExecuteAsync(string uri)
        {
            await _client.PostAsync(uri, null);
        }
        protected virtual async Task<ICommandResult<T>> ExecuteAsync<T>(string uri, object model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(uri, model);

            return await GetResultAsync<T>(response);
        }
        protected virtual async Task ExecuteAsync(string uri, object model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(uri, model);
        }
        #endregion

        #region Helping methods
        protected virtual void ApplyJWTToken()
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["Oauth_token"];

            string token;
            if (cookieReq != null)
            {
                token = cookieReq["token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected virtual void ApplyJsonAccept()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected virtual async Task<ICommandResult<T>> GetResultAsync<T>(HttpResponseMessage response)
        {
            T entity = default(T);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    entity = await response.Content.ReadAsAsync<T>();
                }
                catch { }
                
                var result = new BaseCommandResult<T>(response, entity);
                return result;
            }
            else
            {
                string error = string.Empty;

                using (var stream = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    stream.BaseStream.Position = 0;
                    error = stream.ReadToEnd();
                }

                var result = new BaseCommandResult<T>
                    (
                    response: response,
                    code: (int)response.StatusCode,
                    error: error
                    );

                return result;
            }
        }
        #endregion
    }
}
