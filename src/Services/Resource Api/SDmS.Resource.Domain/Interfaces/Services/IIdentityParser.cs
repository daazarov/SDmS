
using System.Security.Principal;

namespace SDmS.Resource.Domain.Interfaces.Services
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
