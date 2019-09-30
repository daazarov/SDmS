using Microsoft.AspNet.Identity;
using SDmS.Identity.Api.Mapping;
using SDmS.Identity.Api.Models;
using SDmS.Identity.Common.Entities;
using SDmS.Identity.Core.Interfaces.Services;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using SDmS.Identity.Core.Constants;
using SDmS.Identity.Api.Application.Attributes.Filters;
using System.Net;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace SDmS.Identity.Api.Controllers
{
    [RoutePrefix("api/accounts")]
    [ValidatorModelFilter]
    public class AccountController : IdentityBaseApiController
    {
        public AccountController(IUserManager<ApplicationUser> userManager, IRoleManager<ApplicationRole> roleManager)
            : base(userManager, roleManager)
        {
        }

        [AllowAnonymous]
        [Route("forgot_password"), HttpPost]
        public async Task<IHttpActionResult> ForgotPassword([FromBody]AccountEmailModel model)
        {
            throw new System.Exception("Test Exception");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "User not found or email is not confirmed");
                    return BadRequest(ModelState);
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = string.Format(WebUtility.UrlDecode(model.CallbackUrl), model.Email, WebUtility.UrlEncode(code));
                await _userManager.SendEmailAsync(user.Id, "Password Reset", LoadTemplate(callbackUrl, "password-reset.html"));

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [Route("reset_password"), HttpPost]
        public async Task<IHttpActionResult> ResetPassword([FromBody]AccountResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, WebUtility.UrlDecode(model.Code), model.Password);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("confirm_email"), HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this._userManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        #region [Account CRUD]
        [AllowAnonymous]
        [Route(""), HttpPost]
        public async Task<IHttpActionResult> Registration([FromBody]AccountRegistrationModel registrationModel)
        {
            var user = registrationModel.ViewToDomain();
            IdentityResult addUserResult = await this._userManager.CreateAsync(user, registrationModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            string code = await this._userManager.GenerateEmailConfirmationTokenAsync(user.Id);

            string callbackUrl = string.Format(WebUtility.UrlDecode(registrationModel.ConfirmCallbackUrl), user.Id, WebUtility.UrlEncode(code));

            await this._userManager.SendEmailAsync(user.Id, "Confirm your account", LoadTemplate(callbackUrl, "email-confirmation.html"));

            return Created("", user.CreaterDomainToView());
        }

        [HttpGet]
        [Authorize]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> GetAccountById(string id)
        {
            if (id != GetCurrentUserId() || !User.IsInRole("Admin"))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var result = await _userManager.FindByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.CreaterDomainToView());
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteAccountById(string id)
        {
            if (id != GetCurrentUserId() || !User.IsInRole("Admin"))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new NoContentResult(Request);
            }

            await _userManager.DeleteAsync(user);

            return new NoContentResult(Request);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("")]
        public async Task<IHttpActionResult> GetAccountsByFilter([FromUri]AccountRequestModel model)
        {
            if (model == null)
            {
                model = new AccountRequestModel { limit = 20, offset = 0 };
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    return Ok(user.CreaterDomainToView());
                }
                return NotFound();
            }
            else if (!string.IsNullOrEmpty(model.Username))
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    return Ok(user.CreaterDomainToView());
                }
                return NotFound();
            }

            // If there are no conditions, then display all users
            var users = GetCollectionResult<AccountCreatedResponseModel>(
                getCount: () => _userManager.Users.Count(),
                getItems: () => _userManager.Users.ToList().Select(x => x.CreaterDomainToView()),
                modelState: ModelState);

            return Ok(users);
        }
        #endregion

        #region Help methods
        private string LoadTemplate(string url, string templateName)
        {
            string path = HostingEnvironment.MapPath(string.Format(@"~/Content/Templates/{0}", templateName));

            using (var sr = File.OpenText(path))
            {
                var sb = sr.ReadToEnd();
                sr.Close();
                sb = sb.Replace("#URL#", url);
                sb = sb.Replace("#SITE_NAME#", AppConstants.SiteName);
                sb = sb.Replace("#SITE_URL#", AppConstants.SiteUrl);

                return sb;
            }
        }

        private string GetCurrentUserId()
        {
            if (this.User.Identity is ClaimsIdentity)
            {
                var identity = this.User.Identity as ClaimsIdentity;

                string id = identity.FindFirst(x => x.Type == ClaimTypes.Sid)?.Value;

                return id;
            }

            return string.Empty;
        }
        #endregion
    }
}
