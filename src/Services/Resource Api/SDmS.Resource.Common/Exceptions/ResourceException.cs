using System;

namespace SDmS.Resource.Common.Exceptions
{
    public class ResourceException : Exception
    {
        private int _errorCode;
        private int _httpResponseCode;

        public int ErrorCode  => _errorCode;
        public int HttpResponseCode => _httpResponseCode;

        public ResourceException(int errorCode, int httpResponseCode)
        {
            this._errorCode = errorCode;
            this._httpResponseCode = httpResponseCode;
        }

        public ResourceException(string Message, int errorCode, int httpResponseCode) : base(Message)
        {
            this._errorCode = errorCode;
            this._httpResponseCode = httpResponseCode;
        }

        public ResourceException(string Message, Exception innerException, int errorCode, int httpResponseCode) : base(Message, innerException)
        {
            this._errorCode = errorCode;
            this._httpResponseCode = httpResponseCode;
        }
    }
}
