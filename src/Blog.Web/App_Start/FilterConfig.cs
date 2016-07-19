using System.Web.Mvc;
using Blog.Web.Infrastructure;

namespace Blog.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LastModifiedCacheAttribute());
        }
    }
}