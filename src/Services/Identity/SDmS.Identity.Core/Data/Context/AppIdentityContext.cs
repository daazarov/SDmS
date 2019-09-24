using Microsoft.AspNet.Identity.EntityFramework;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Data;
using System.Data.Entity;

namespace SDmS.Identity.Core.Data.Context
{
    public class AppIdentityContext : IdentityDbContext<ApplicationUser>, IAppIdentityContext
    {
        public AppIdentityContext(string connectionString) : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
