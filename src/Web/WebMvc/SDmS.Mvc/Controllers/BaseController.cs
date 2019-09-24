using SDmS.Mvc.Domain.Core.Constants;
using SDmS.Mvc.Models;
using System.Web.Mvc;

namespace SDmS.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void ShowMessage(GenericMessageViewModel messageViewModel)
        {
            TempData[AppConstants.MessageViewBagName] = messageViewModel;
        }

        protected string GetErrors(ModelStateDictionary models)
        {
            string error_text = string.Empty;

            foreach (var model in models.Values)
            {
                foreach (ModelError error in model.Errors)
                {
                    error_text += error.ErrorMessage + "\n";
                }
            }

            return error_text;
        }
    }
}