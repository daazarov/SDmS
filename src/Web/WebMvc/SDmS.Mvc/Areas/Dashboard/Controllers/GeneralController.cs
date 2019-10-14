using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Mvc.Attributes.Filters;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
    public class GeneralController : BaseDashboardController
    {
        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}