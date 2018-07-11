using System.Web.Optimization;

namespace Better4You.UI.Mvc.Configuration
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void Configure()
        {
            var bundles = BundleTable.Bundles;

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                            .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar")
                            .Include("~/Scripts/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/gcal")
                .Include("~/Scripts/gcal.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                            .Include("~/Scripts/bootstrap.js",
                            "~/Scripts/bootstrap-select.js",
                            "~/Scripts/bootstrap-datepicker.js"
                            ));
            /*
                        bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

             */
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.10.2.custom.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(
                new ScriptBundle("~/bundles/plugins")
                    .Include(
                        "~/Scripts/jquery.maskedinput.js",
                        "~/Scripts/jquery.placeholder.js",
                        "~/plugins/knockout/knockout-v3.3.0-debug.js",
                        "~/plugins/handlebars/handlebars-v2.0.0.js"
                    ));
            
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Scripts/vendor/handlebars-1.0.0-rc.4.js",
                "~/Scripts/vendor/ember-1.0.0-rc.5.js",
                "~/Scripts/vendor/ember-data.js",
                "~/Scripts/vendor/adapter.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/site")
                .Include("~/Scripts/site.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            /*
            bundles.Add(new Bundle("~/Content/less", new LessTransform(), new CssMinify())
                            .Include("~/Content/site.less"));
            */
            bundles.Add(new StyleBundle("~/Content/normalize").Include("~/Content/normalize.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/cus-icons.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/bootstrap-select.css")
                .Include("~/Content/datepicker.css"));

            bundles.Add(new StyleBundle("~/Content/fullcalendar")
                            .Include("~/Content/fullcalendar.css")
                            .Include("~/Content/fullcalendar.print.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
        
            /*
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
             */
        }
    }
}