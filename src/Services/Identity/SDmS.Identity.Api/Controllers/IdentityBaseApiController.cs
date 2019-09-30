using Microsoft.AspNet.Identity;
using SDmS.Identity.Api.Models;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace SDmS.Identity.Api.Controllers
{
    public abstract class IdentityBaseApiController : ApiController
    {
        protected readonly IUserManager<ApplicationUser> _userManager;
        protected readonly IRoleManager<ApplicationRole> _roleManager;

        public IdentityBaseApiController(IUserManager<ApplicationUser> userManager, IRoleManager<ApplicationRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("*", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected ResponseCollectionModel<T> GetCollectionResult<T>(Func<IEnumerable<T>> getItems, Func<int> getCount, ModelStateDictionary modelState)
        {
            ResponseCollectionModel<T> result = new ResponseCollectionModel<T>();

            result.TotalCount = getCount();

            result.Collection = new List<T>();

            foreach (var user in getItems())
            {
                result.Collection.Add(user);
            }

            return result;
        }

        protected ResponseModel<T> GetResult<T>(Func<T> getItem, ModelStateDictionary modelState)
        {
            ResponseModel<T> result = new ResponseModel<T>();

            result.Value = getItem();

            return result;
        }

        public class NoContentResult : IHttpActionResult
        {
            private readonly HttpRequestMessage _request;
            private readonly string _reason;

            public NoContentResult(HttpRequestMessage request, string reason)
            {
                _request = request;
                _reason = reason;
            }

            public NoContentResult(HttpRequestMessage request)
            {
                _request = request;
                _reason = "No Content";
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = _request.CreateResponse(HttpStatusCode.NoContent, _reason);
                return Task.FromResult(response);
            }
        }
    }
}
