using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDmS.Resource.Infrastructure.Interfaces;
using SDmS.Resource.Infrastructure.Services.Data;
using SDmS.Resource.Infrastructure.Services.Data.Context;

namespace SDmS.Resource.DI.Modules
{
    public class DataModule : IModule
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ResourceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ResourceConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
        }
    }
}
