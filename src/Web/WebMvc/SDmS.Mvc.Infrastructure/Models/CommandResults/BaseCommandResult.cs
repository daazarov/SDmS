using System.Net;
using System.Net.Http;
using SDmS.Infrastructure.Interfaces;

namespace SDmS.Infrastructure.Models
{
    public class BaseCommandResult<T> : ICommandResult<T>
    {
        private readonly HttpResponseMessage _response;
        private readonly T _value;
        private readonly string _error;
        private readonly int _code;

        public BaseCommandResult(HttpResponseMessage response = null, T value = default(T), int? code = null, string error = null)
        {
            this._response = response;
            this._value = value;
            this._error = error;
            if (code.HasValue)
                this._code = code.Value;
        }

        public HttpStatusCode Code
        {
            get
            {
                if (_response != null)
                    return _response.StatusCode;
                return (HttpStatusCode)_code;
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
        }

        public bool HasValue
        {
            get
            {
                return Value != null;
            }
        }

        public HttpResponseMessage Response
        {
            get
            {
                return _response;
            }
        }

        public T Value
        {
            get
            {
                return _value; ;
            }
        }
    }
}