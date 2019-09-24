using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace SDmS.Identity.Core.Interfaces.Services
{
    public interface IRoleManager<IRole>
    {
        IQueryable<IRole> Roles { get; }

        Task<IdentityResult> CreateAsync(IRole role);
        Task<IdentityResult> DeleteAsync(IRole role);
        Task<IRole> FindByIdAsync(string roleId);
        Task<IRole> FindByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
