using System.Net;
using System.Net.Http;

namespace SDmS.Infrastructure.Interfaces
{
    public interface ICommandResult<T>
    {
        bool HasValue { get; }
        HttpResponseMessage Response { get; }
        HttpStatusCode ResponseCode { get; }
        string Error { get; }
        T Value { get; }
    }
}
