using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    public class LightsController : BaseDashboardController
    {
        // GET: Dashboard/Lights
        public ActionResult Index()
        {
            return View();
        }
    }
}