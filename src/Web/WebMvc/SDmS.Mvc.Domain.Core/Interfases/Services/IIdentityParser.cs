using System.Security.Principal;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
