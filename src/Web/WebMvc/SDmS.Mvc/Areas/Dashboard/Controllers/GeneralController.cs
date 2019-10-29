using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SDmS.Domain.Core.Interfases.Services;
using SDmS.Mvc.Attributes.Filters;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    //[DashboardAuthorization]
    public class GeneralController : BaseDashboardController
    {
        public GeneralController(ILoggingService loggingService) : base(loggingService)
        {
        }

        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}