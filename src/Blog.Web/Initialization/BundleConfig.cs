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
            css.Include("~/content/css/font-awesome.css", new CssRewriteUrlTransform());
            css.Include("~/content/css/kube.min.css");
            css.Include("~/content/css/kube.responsive.min.css");
            css.Include("~/content/css/forms.css");
            css.Include("~/content/css/site.css");

            bundles.Add(css);
        }
    }
}