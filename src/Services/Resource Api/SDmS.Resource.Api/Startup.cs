using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SDmS.Resource.Api.Extensions;
using SDmS.Resource.Api.OAuth;
using SDmS.Resource.Common;
using SDmS.Resource.DI;
using SDmS.Resource.DI.Modules;

namespace SDmS.Resource.Api
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

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            RegisterComponent<DataModule>(services, Configuration);
            RegisterComponent<DomainServicesModule>(services, Configuration);
            RegisterComponent<InfrastructureServicesModule>(services, Configuration);
            RegisterComponent<ModelMapperModule>(services, Configuration);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.IncludeErrorDetails = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["ISSUER"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["AUDIENCE"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(Configuration["SECRET_KEY"]),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromSeconds(20)
                        };
                    });

            IEndpointInstance endpointInstance = null;
            services.AddSingleton<IEndpointInstance>(_ => endpointInstance);

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

            endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            return container;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMvc();

            loggerFactory.AddFile("Logs/resource_api-{Date}.txt");
        }

        private void RegisterComponent<T>(IServiceCollection services, IConfiguration configuration) where T : IModule, new()
        {
            new T().Register(services, configuration);
        }
    }
}
