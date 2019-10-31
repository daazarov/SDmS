using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SDmS.Resource.Api.Models;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;
using System;

namespace SDmS.Resource.Api.Attributes.Filters
{
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IErrorInformatorService _errorInformatorService;

        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger, IErrorInformatorService errorInformatorService)
        {
            this._logger = logger;
            this._errorInformatorService = errorInformatorService;
        }

        public void OnException(ExceptionContext context)
        {
            ResponseModel response;

            if (context.Exception is ResourceException exception)
            {
                string error = String.Empty;
                try
                {
                    error = this._errorInformatorService.GetDescription(exception.ErrorCode);
                }
                catch
                {
                    error = "Unidentified error";
                }

                response = new ResponseModel
                {
                    HttpResponseCode = exception.HttpResponseCode,
                    ErrorCode = exception.ErrorCode,
                    Error = error
                };

                context.Result = new JsonResult(response);
                context.HttpContext.Response.StatusCode = exception.HttpResponseCode;
                context.ExceptionHandled = true;

                return;
            }

            _logger.LogCritical(context.Exception, "Unhandled exception");

            response = new ResponseModel
            {
                HttpResponseCode = 500,
                ErrorCode = -999,
                Error = "Unidentified error"
            };

            context.Result = new JsonResult(response);
            context.HttpContext.Response.StatusCode = 500;
            context.ExceptionHandled = true;
        }
    }
}
