using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using SDmS.Identity.Api.Providers;
using SDmS.Identity.Application.Extensions;
using SDmS.Identity.DI;

[assembly: OwinStartup(typeof(SDms.Identity.Api.Startup))]
namespace SDms.Identity.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var kernel = NinjectHelper.GetNinjectResolver();

            HttpConfiguration httpConfig = new HttpConfiguration();

            app.UseSwagger(httpConfig);

            app.UseNinjectMiddleware(() => kernel);

            ConfigureOAuthTokenGeneration(app);

            ConfigureOAuthTokenConsumption(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.ConfigureWebApi(httpConfig);

            app.RegisterFilters(httpConfig);

            app.UseNinjectWebApi(httpConfig);

            app.PreLoadIdentityContext();
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = Activator.CreateInstance<CustomOAuthProvider>(),
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["as:Issuer"])
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = ConfigurationManager.AppSettings["as:Issuer"];
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}