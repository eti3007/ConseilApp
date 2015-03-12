using System.Web;
using System.Web.Optimization;

namespace ConseilApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            // _master
            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                        "~/Content/Bootstrap/css/bootstrap.css",
                        "~/Content/Bootstrap/css/bootstrap-responsive.css"));
            bundles.Add(new ScriptBundle("~/Bundles/Bootstrap/a").Include(
                        "~/Scripts/JQuery/jquery-min.js",
                        "~/Scripts/JQuery/jquery-ui-min.js",
                        "~/Scripts/Custom/Menu.js"));
            // Login - Register - Manage
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/InputFile").Include(
                        "~/Scripts/Bootstrap/bootstrap-filestyle-min.js",
                        "~/Scripts/Bootstrap/bootstrap.file-input.js"));


            bundles.Add(new StyleBundle("~/Content/Custom")
                .Include("~/Content/Custom/Custom.css"));
            
            bundles.Add(new StyleBundle("~/Content/SlideCss").Include(
                "~/Content/Carousel/tango/skin.css",
                "~/Content/Carousel/jquery.jcarousel.css"));
            bundles.Add(new ScriptBundle("~/bundles/SlideJs").Include(
                "~/Scripts/jquery.jcarousel.js",
                "~/Scripts/Custom/UploadPhotos.js",
                "~/Scripts/Custom/Carousel.js"));
            bundles.Add(new ScriptBundle("~/bundles/CarouselJs").Include(
                "~/Scripts/jquery.jcarousel.js",
                "~/Scripts/Custom/Carousel.js"));
            bundles.Add(new ScriptBundle("~/bundles/RechercheJs").Include(
                "~/Scripts/Custom/Recherche.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/vendor")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.jcarousel.js")
                .Include("~/Scripts/lytebox.js"));

            bundles.Add(new StyleBundle("~/Content/vendor")
                .Include("~/Content/Carousel/lytebox.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
/*            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/vendor")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.jcarousel.js")
                .Include("~/Scripts/lytebox.js")
                .Include("~/Scripts/Custom/HomeScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/vendor")
                .Include("~/Content/Carousel/tango/skin.css")
                .Include("~/Content/Carousel/jquery.jcarousel.css")
                .Include("~/Content/Carousel/lytebox.css"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/Custom.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));*/