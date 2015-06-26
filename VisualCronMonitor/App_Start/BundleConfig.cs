using System.Web.Optimization;

namespace VisualCronMonitor
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include( "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/js/lib").IncludeDirectory("~/Scripts/lib", "*.js", false));
            bundles.Add(new ScriptBundle("~/bundles/js/monitor").IncludeDirectory("~/Scripts/monitor", "*.js", false));
            bundles.Add(new StyleBundle("~/Content/css").Include( "~/Content/bootstrap.css", "~/Content/site.css"));
        }
    }
}
