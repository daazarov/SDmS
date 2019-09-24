using System.Web.Mvc;
using System.Web.Routing;

namespace SDmS.Mvc.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            RouteTable.Routes.LowercaseUrls = true;

            context.MapRoute(
                "Dashboard_default",
                "Dashboard/{controller}/{action}/{id}",
                new { controller = "General", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}