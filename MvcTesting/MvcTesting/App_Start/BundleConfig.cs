using System.Web;
using System.Web.Optimization;

namespace MvcTesting
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Script Bundles
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/jquery").Include("~/Scripts/jquery-1.8.3.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/jquery-ui").Include("~/Scripts/jquery-ui-1.9.2.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/jqueryUploader").Include("~/Scripts/jquery.fileupload.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/jqueryUploaderIframe").Include("~/Scripts/jquery.iframe-transport.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include("~/Scripts/bootstrap.js"));
            

            // Style Bundles
            BundleTable.Bundles.Add(new StyleBundle("~/Styles/base").Include("~/Content/bootstrap.css",
                "~/Content/jquery.fileupload-ui-noscript.css",
               "~/Content/jquery.fileupload-ui.css",
               "~/Content/themes/base/jquery-ui*",
               "~/Content/themes/base/jquery.ui*"));
        }
    }
}