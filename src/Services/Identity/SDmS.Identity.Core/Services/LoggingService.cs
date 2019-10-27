using SDmS.Identity.Core.Interfaces.Services;
using System;
using System.Text;
using SDmS.Common.Models;
using SDmS.Identity.Core.Interfaces.Data;
using SDmS.Identity.Core.Data.Context;
using SDmS.Common.Entities;

namespace SDmS.Identity.Core.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly AppIdentityContext _identityContext;

        public LoggingService(IAppIdentityContext identityContext)
        {
            this._identityContext = identityContext as AppIdentityContext;
        }

        public void Error(Exception exception, ActionInfo action)
        {
            if (exception == null)
            {
                return;
            }

            var message = new StringBuilder(exception.Message);
            var inner = exception.InnerException;
            var depthCounter = 0;
            int maxExceptionDepth = 5;

            while (inner != null && depthCounter++ < maxExceptionDepth)
            {
                message.Append(" INNER EXCEPTION: ");
                message.Append(inner.Message);
                inner = inner.InnerException;
            }

            AppException row = new AppException
            {
                Action = (action != null) ? action.ActionName : "",
                Controller = (action != null) ? action.ControllerName : "",
                Message = message.ToString(),
                StackTrace = exception.StackTrace
            };

            _identityContext.AppExceptions.Add(row);

            _identityContext.SaveChanges();
        }
    }
}
