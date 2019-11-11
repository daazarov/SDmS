using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace SDmS.Resource.Infrastructure.Services.Data.Context
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Enable LazyLoading for EFCore
            // need package Microsoft.EntityFrameworkCore.Proxies
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                                    .Where(type => !string.IsNullOrEmpty(type.Namespace))
                                    .Where(type => type.BaseType != null && type.IsClass
                                    && type.Namespace == "SDmS.Resource.Infrastructure.Services.Data.EntityMapping"
                                    && !type.IsNested).ToList()
                                    /*.Where(type => type.GetInterfaces().Contains(typeof(IEntityTypeConfiguration<>)))*/;

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
