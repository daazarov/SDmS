using Microsoft.AspNet.Identity;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

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
