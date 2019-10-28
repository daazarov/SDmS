using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SDmS.Resource.Api.Models;
using SDmS.Resource.Common.Exceptions;
using SDmS.Resource.Domain.Interfaces.Services;

namespace SDmS.Resource.Api.Controllers.v1
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IErrorInformatorService _errorInformatorService;

        public BaseApiController(IErrorInformatorService errorInformatorService)
        {
            this._errorInformatorService = errorInformatorService;
        }

        protected ResponseCollectionModel<T> GetCollectionResult<T>(Func<IEnumerable<T>> getItems, Func<int> getCount, ModelStateDictionary modelState)
        {
            ResponseCollectionModel<T> result = new ResponseCollectionModel<T>();

            try
            {
                result.TotalCount = getCount();

                result.Collection = new List<T>();

                foreach (var user in getItems())
                {
                    result.Collection.Add(user);
                }
            }
            catch (ResourceException ex)
            {
                var error = this._errorInformatorService.GetDescription(ex.ErrorCode);
                result.Error = error;
                result.ErrorCode = ex.ErrorCode;
                result.HttpResponseCode = ex.HttpResponseCode;
            }
            catch (Exception ex)
            {
                result.Error = "Unidentified error";
                result.ErrorCode = -999;
                result.HttpResponseCode = 500;
            }

            return result;
        }

        protected ResponseModel<T> GetResult<T>(Func<T> getItem, ModelStateDictionary modelState)
        {
            ResponseModel<T> result = new ResponseModel<T>();

            try
            {
                result.Value = getItem();
            }
            catch (ResourceException ex)
            {
                var error = this._errorInformatorService.GetDescription(ex.ErrorCode);
                result.Error = error;
                result.ErrorCode = ex.ErrorCode;
                result.HttpResponseCode = ex.HttpResponseCode;
            }
            catch (Exception ex)
            {
                result.Error = "Unidentified error";
                result.ErrorCode = -999;
                result.HttpResponseCode = 500;
            }

            return result;
        }
    }
}