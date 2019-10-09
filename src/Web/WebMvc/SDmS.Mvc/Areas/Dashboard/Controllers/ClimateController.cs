using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Mvc.Attributes.Filters;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    [DashboardAuthorization]
	public class ClimateController : BaseDashboardController
    {
        // GET: Dashboard/Lights
        public ActionResult Index()
        {
            return View();
        }
    }
}