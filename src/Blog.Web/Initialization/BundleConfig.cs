using System.Web.Optimization;

namespace Blog.Web.Initialization
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.DirectoryFilter.Clear();
            bundles.DirectoryFilter.Ignore("font-awesome.*", OptimizationMode.Always);

            var css = new StyleBundle("~/bundles/css");
            css.IncludeDirectory("~/content/css", "*.css");
            css.Include("~/content/css/font-awesome.min.css", new CssRewriteUrlTransform());

            bundles.Add(css);
        }
    }
}