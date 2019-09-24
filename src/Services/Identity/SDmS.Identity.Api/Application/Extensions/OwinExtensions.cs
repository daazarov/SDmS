using FluentValidation.WebApi;
using Newtonsoft.Json.Serialization;
using Owin;
using SDmS.Identity.Core.Interfaces.Services;
using SDmS.Identity.DI;
using Swashbuckle.Application;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using SDmS.Identity.Api.Application.Attributes.Filters;

namespace SDmS.Identity.Application.Extensions
{
    public static class OwinExtensions
    {
        public static void UseSwagger(this IAppBuilder @this, HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "SDmS.Identity.Api");

                //c.IncludeXmlComments(string.Format(@"{0}\bin\SDmS.Identity.Api.XML",
                //   System.AppDomain.CurrentDomain.BaseDirectory));

                c.DescribeAllEnumsAsStrings();
            })
            .EnableSwaggerUi();
        }

        /// <summary>
        /// Use after injected dependency
        /// </summary>
        public static void PreLoadIdentityContext(this IAppBuilder @this)
        {
            var service = (IIdentityInitializationService)NinjectHelper.GetResolveService(typeof(IIdentityInitializationService));
            service.InitializeAsync();
        }

        public static void ConfigureWebApi(this IAppBuilder @this, HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            FluentValidationModelValidatorProvider.Configure(config);

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static void RegisterFilters(this IAppBuilder @this, HttpConfiguration config)
        {
            // register global filters
            config.Filters.Add(new ApiExceptionFilterAttribute());
        }
    }
}