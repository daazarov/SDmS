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
using System.Web;
using System.Net;

namespace SDmS.Identity.Api.Controllers
{
    [RoutePrefix("api/accounts")]
    [ValidatorModelFilter, ApiExceptionFilter]
    public class AccountController : IdentityBaseApiController
    {
        public AccountController(IUserManager<ApplicationUser> userManager, IRoleManager<ApplicationRole> roleManager)
            : base(userManager, roleManager)
        {
        }

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

        [AllowAnonymous]
        [Route("forgot_password"), HttpPost]
        public async Task<IHttpActionResult> ForgotPassword([FromBody]AccountEmailModel model)
        {
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
        #endregion
    }
}
