using System.Web;
using System.Web.Optimization;

namespace SDmS.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery/jquery-{version}.js",
                        "~/Content/js/jquery/jquery.unobtrusive-ajax.js",
                        "~/Content/js/jquery/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/Content/dashboard/js").Include(
                      "~/Content/js/jquery-ui-slider-pips.js"));

            bundles.Add(new ScriptBundle("~/Content/uikit/js").Include(
                      "~/Content/js/uikit/uikit.js",
                      "~/Content/js/uikit/uikit-icons.js"));

            bundles.Add(new StyleBundle("~/Content/dashboard/css").Include(
                      "~/Content/css/site.css",
                      "~/Content/css/dashboard.css",
                      "~/Content/css/toogle-button.css",
                      "~/Content/css/jquery-ui/jquery-ui-slider-pips.css",
                      "~/Content/css/range-slider.css",
                      "~/Content/css/btn-add-device.css",
                      "~/Content/css/jquery-ui/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/uikit/css").Include(
                      "~/Content/css/uikit/uikit.min.css"));

            bundles.Add(new StyleBundle("~/Content/uikit-dashboard/css").Include(
                      "~/Content/css/uikit/uikit.min.dashboard.css"));
        }
    }
}
