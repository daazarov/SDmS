using Microsoft.AspNet.Identity.EntityFramework;

namespace SDmS.Identity.Common.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }
    }
}
