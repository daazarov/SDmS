using SDmS.Domain.Core.Interfases.Services;
using SDmS.Mvc.Models.Account;
using System.Web;
using System.Web.Mvc;
using SDmS.Mvc.Mappers;
using System.Threading.Tasks;
using SDmS.Mvc.Models.Enums;
using SDmS.Domain.Core.Models.Account;
using System;
using SDmS.Mvc.Domain.Core.Constants;
using System.Web.Routing;
using System.Net;

namespace SDmS.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMembershipService _membershipService;
        private readonly ILoggingService _loggingService;

        public AccountController(IMembershipService membershipService, ILoggingService loggingService)
        {
            this._membershipService = membershipService;
            this._loggingService = loggingService;
        }

        #region [SingUp/LogOff]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginViewModel model)
        {
            var result = await this._membershipService.SingUpAsync(model.ViewToDomain());
            if (result.Code != 200)
            {
                ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = result.Error });
                return View(model);
            }

            HttpCookie cookie = new HttpCookie(AppConstants.TokenCookieName);

            cookie["token"] = result.Value.Token;
            cookie.Expires = result.Value.Expires;

            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logoff()
        {
            if (Request.Cookies[AppConstants.TokenCookieName] != null)
            {
                var cookie = new HttpCookie(AppConstants.TokenCookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region [ResetPassword]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(AccountEmailModel model)
        {
            if (!ModelState.IsValid)
            {

            }

            var callbackUrl = Url.Action("ResetPassword", "Account", new { email = "{0}", code = "{1}" }, Request.Url.Scheme);
            model.CallbackUrl = callbackUrl;

            await _membershipService.ForgotPasswordAsync(model.Email, model.CallbackUrl);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string email, string code)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(email))
            {
                ShowMessage(new Models.GenericMessageViewModel { Message = "Reset password token or email can not be empty", Type = MessageTypes.warning });
                return RedirectToAction(nameof(Login));
            }
            ViewBag.Code = code;
            ViewBag.Email = email;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string code, AccountChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Code = code;
                ViewBag.Email = model.Email;
                return View(model);
            }

            var result = await _membershipService.ResetPasswordAsync(new AccountResetPasswordModel { Code = WebUtility.UrlEncode(code), Password = model.Password, Email = model.Email});

            if (result.Code != 200)
            {
                ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = result.Error });
                return View(model);
            }

            ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.success, Message = $"Password was successfully changed" });
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region [Registration]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(AccountRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var callbackUrl = /*HttpUtility.HtmlEncode(*/Url.Action("ConfirmEmail", "Account", new { userId = "{0}", code = "{1}" }, Request.Url.Scheme);
            model.ConfirmCallbackUrl = callbackUrl;

            var result = await _membershipService.RegistrationAsync(model.ViewToDomain());

            if (result.Code != 201)
            {
                ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = result.Error });
                return View(model);
            }

            ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.success, Message = $"A confirmation email was sent to your email {result.Value.Email}" });
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(AccountConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = GetErrors(ModelState) });
            }

            var result = await _membershipService.ConfirmEmailAddresssAsync(model.UserId, WebUtility.UrlEncode(model.code));

            if (result.Code != 200)
            {
                ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = result.Error });
                return RedirectToAction(nameof(Login));
            }

            ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.success, Message = "Email confirmed, thanks for registering" });

            return RedirectToAction(nameof(Login));
        }
        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            ShowMessage(new Models.GenericMessageViewModel { Type = MessageTypes.warning, Message = " 500 Internal Server Error. Please try again later" });

            filterContext.Result = this.RedirectToAction("Login", "Account");
            filterContext.ExceptionHandled = true;

            try
            {
                _loggingService.Error(filterContext.Exception);
            }
            catch { }
        }
    }
}