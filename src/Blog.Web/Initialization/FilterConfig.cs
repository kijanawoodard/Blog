using System.Web.Mvc;
using Blog.Web.Infrastructure;

namespace Blog.Web.Initialization
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LastModifiedCacheAttribute());
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
        }
    }
}