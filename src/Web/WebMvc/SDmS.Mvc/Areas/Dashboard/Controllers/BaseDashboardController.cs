using SDmS.Domain.Core.Interfases.Services;
using SDmS.Mvc.Domain.Core.Constants;
using SDmS.Mvc.Models;
using SDmS.Mvc.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDmS.Mvc.Areas.Dashboard.Controllers
{
    public abstract class BaseDashboardController : Controller
    {
        protected readonly ILoggingService _loggingService;

        public BaseDashboardController(ILoggingService loggingService)
        {
            this._loggingService = loggingService;
        }

        protected void ShowMessage(GenericMessageViewModel messageViewModel)
        {
            TempData[AppConstants.MessageViewBagName] = messageViewModel;
        }
    }
}