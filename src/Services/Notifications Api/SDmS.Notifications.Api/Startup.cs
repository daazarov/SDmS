using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SDmS.Notifications.Api.Extensions;

namespace SDmS.Notifications.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });

            services.AddScoped<ReusedComponent>();

            services.AddTransport();

            var endpointConfiguration = services.AddNServiceBus(Configuration);

            #region Create IoC container
            UpdateableServiceProvider container = null;
            endpointConfiguration.UseContainer<ServicesBuilder>(customizations =>
            {
                customizations.ExistingServices(services);
                customizations.ServiceProviderFactory(sc =>
                {
                    container = new UpdateableServiceProvider(sc);
                    return container;
                });
            });
            #endregion

            NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            return container;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
