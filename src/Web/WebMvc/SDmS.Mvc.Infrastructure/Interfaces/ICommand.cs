using System.Threading.Tasks;

namespace SDmS.Infrastructure.Interfaces
{
    public interface ICommand
    {
        string CommandName { get; }
        Task RunAsync(object properties);
        Task RunAsync(string uri);
        Task RunAsync(string uri, object properties);
        Task<ICommandResult<T>> RunAsync<T>(object properties);
        Task<ICommandResult<T>> RunAsync<T>(string uri);
        Task<ICommandResult<T>> RunAsync<T>(string uri, object properties);
    }
}
