using System.Web;
using System.Web.Optimization;

namespace onlinePlatby
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true; // Make true to see release styles
            bundles.UseCdn = true;   // Enable CDN support

            var jqueryCdnPath = "https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js";
            var bootstrapCdnPath = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css";
            var bootstrapCdnJsPath = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery",
                jqueryCdnPath).Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap",
                bootstrapCdnJsPath).Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap",
                bootstrapCdnPath).Include(
                "~/Content/css/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // YOUR NEW BUNDLES...

        }
    }
}
