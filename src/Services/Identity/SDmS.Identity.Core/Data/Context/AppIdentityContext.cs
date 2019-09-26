using Microsoft.AspNet.Identity.EntityFramework;
using SDmS.Common.Entities;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.EntityMapping;
using SDmS.Identity.Core.Interfaces.Data;
using System.Data.Entity;

namespace SDmS.Identity.Core.Data.Context
{
    public class AppIdentityContext : IdentityDbContext<ApplicationUser>, IAppIdentityContext
    {
        public DbSet<AppException> AppExceptions { get; set; }

        public AppIdentityContext(string connectionString) : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppExceptionMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
