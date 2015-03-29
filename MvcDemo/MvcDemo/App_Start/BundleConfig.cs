using System.Web.Optimization;

namespace MvcDemo {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/common.js",
                    "~/Scripts/featherlight.min.js"
//                    "~/Scripts/jquery.magnific-popup.js"
            ));



            bundles.Add(new StyleBundle("~/bundles/styles").Include(
//                      "~/Content/bootstrap.css",
                    "~/Content/themes/flatly/bootstrap.css",
                    "~/Content/site.css",
                    "~/Content/themes/base/*.css",
                    "~/Content/featherlight.min.css" 
//                    "~/Content/magnific-popup.css", 
                    
                    ));




            BundleTable.EnableOptimizations = true;
        }
    }
}
