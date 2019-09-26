using SDmS.Common.Models;
using SDmS.Identity.Core.Interfaces.Services;
using SDmS.Identity.DI;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SDmS.Identity.Api.Application.Attributes.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private ILoggingService _loggingService;

        private ILoggingService LoggingService
        {
            get
            {
                if (_loggingService == null)
                {
                    _loggingService = (ILoggingService)NinjectHelper.GetResolveService(typeof(ILoggingService));
                }
                return _loggingService;
            }
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception != null)
            {
                var message = "Oops, something went wrong";
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);

                ActionInfo info = new ActionInfo
                {
                    ControllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName
                };

                try
                {
                    LoggingService.Error(actionExecutedContext.Exception, info);
                }
                catch { }
            }
            return Task.FromResult<object>(null);
        }
    }
}