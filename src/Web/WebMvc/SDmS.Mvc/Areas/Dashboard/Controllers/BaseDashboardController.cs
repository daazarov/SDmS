using SDmS.Mvc.Domain.Core.Constants;
using SDmS.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    public abstract class BaseDashboardController : Controller
    {
        protected void ShowMessage(GenericMessageViewModel messageViewModel)
        {
            TempData[AppConstants.MessageViewBagName] = messageViewModel;
        }
    }
}