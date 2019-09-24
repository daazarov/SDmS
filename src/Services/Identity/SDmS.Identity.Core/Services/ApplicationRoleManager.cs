using Microsoft.AspNet.Identity;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;

namespace SDmS.Identity.Core.Services
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IRoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }
    }
}
