using System.Web;
using System.Web.Mvc;

namespace SDmS.Mvc.Attributes.Filters
{
    public class DashboardAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            return isAuthorized;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.RequestContext.RouteData.Values["controller"] = "Account";
            filterContext.RequestContext.RouteData.Values["action"] = "Login";
        }
    }
}