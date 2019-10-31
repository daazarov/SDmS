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
    public class DeleteCommand : ICommand
    {
        private const string NAME = "BASE_DELETE_COMMAND";

        private readonly string _commandName;
        private string _baseUri;
        protected readonly HttpClient _client;

        public bool UseToken { get; protected set; } = true;
        public bool UseJson { get; protected set; } = true;

        public DeleteCommand(string commandName, string baseUri = null)
        {
            this._commandName = commandName;
            this._baseUri = baseUri;
            _client = new HttpClient();
        }
        public DeleteCommand() :this(NAME, null)
        { }

        #region Interface implimentation
        public string CommandName => _commandName;

        public async Task RunAsync(object properties)
        {
            if (UseToken) ApplyJWTToken();
            if (UseJson) ApplyJsonAccept();

            await ExecuteAsync(properties);
        }

        public async Task RunAsync(string uri)
        {
            if (UseToken) ApplyJWTToken();
            if (UseJson) ApplyJsonAccept();

            await ExecuteAsync(uri);
        }

        public async Task<ICommandResult<T>> RunAsync<T>(object properties)
        {
            if (UseToken) ApplyJWTToken();
            if (UseJson) ApplyJsonAccept();

            ICommandResult<T> result = await ExecuteAsync<T>(properties);

            return result;
        }

        public async Task<ICommandResult<T>> RunAsync<T>(string uri)
        {
            if (UseToken) ApplyJWTToken();
            if (UseJson) ApplyJsonAccept();

            ICommandResult<T> result = await ExecuteAsync<T>(uri);

            return result;
        }

        #endregion

        #region Not implimented
        public Task<ICommandResult<T>> RunAsync<T>(string uri, object properties)
        {
            throw new NotImplementedException(nameof(RunAsync));
        }
        public Task RunAsync(string uri, object properties)
        {
            throw new NotImplementedException(nameof(RunAsync));
        }
        #endregion

        #region Abstract methods
        protected virtual async Task<ICommandResult<T>> ExecuteAsync<T>(object properties)
        {
            if (string.IsNullOrEmpty(_baseUri))
            {
                throw new ArgumentNullException(nameof(_baseUri));
            }

            string query;

            uriQueryBuilder(properties, out query);

            return await ExecuteAsync<T>(string.Format("{0}?{1}", _baseUri, query));
        }
        protected virtual async Task<ICommandResult<T>> ExecuteAsync<T>(string uri)
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);

            return await GetResultAsync<T>(response);
        }
        protected virtual async Task ExecuteAsync(object properties)
        {
            if (string.IsNullOrEmpty(_baseUri))
            {
                throw new ArgumentNullException(nameof(_baseUri));
            }

            string query;

            uriQueryBuilder(properties, out query);

            await ExecuteAsync(string.Format("{0}?{1}", _baseUri, query));
        }
        protected virtual async Task ExecuteAsync(string uri)
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);
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

        protected virtual bool uriQueryBuilder(object model, out string query)
        {
            List<string> values = new List<string>();
            query = string.Empty;

            if (model != null)
            {
                Type type = model.GetType();
                foreach (var property in type.GetProperties())
                {
                    if (property.CustomAttributes.Any(x => x.AttributeType == typeof(QueryPropertyAttribute)))
                    {
                        object value = property.GetValue(model);
                        if (value == null) continue;

                        values.Add(String.Format("{0}={1}", property.Name, value));
                    }
                }
                query = String.Join("&", values);
            }

            return !String.IsNullOrEmpty(query);
        }

        protected virtual async Task<ICommandResult<T>> GetResultAsync<T>(HttpResponseMessage response)
        {
            T entity = default(T);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    entity = await response.Content.ReadAsAsync<T>();
                }
                catch { }
                
                var result = new BaseCommandResult<T>(response, entity);
                return result;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var result = new BaseCommandResult<T>(response);
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
