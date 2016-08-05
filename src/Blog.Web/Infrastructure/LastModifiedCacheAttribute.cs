using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.Infrastructure
{
    public class LastModifiedCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var now = DateTime.UtcNow;
            //var lastModified = MvcApplication.LastModified;
            var lastModified = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc); //refresh every hour

            if (filterContext.Result is FilePathResult)
            {
                // static content is served from file in my example
                // the last file write time is taken as modification date
                var result = (FilePathResult) filterContext.Result;
                lastModified = new FileInfo(result.FileName).LastWriteTime;
            }

            var isModified = HasModification(filterContext.RequestContext, lastModified);
            if (!isModified) filterContext.Result = NotModified(filterContext.RequestContext, lastModified);

            SetLastModifiedDate(filterContext.RequestContext, lastModified);

            base.OnActionExecuted(filterContext);
        }

        private static void SetLastModifiedDate(RequestContext requestContext, DateTime modificationDate)
        {
            requestContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            requestContext.HttpContext.Response.Cache.SetLastModified(modificationDate);
            requestContext.HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(1));
            requestContext.HttpContext.Response.Cache.SetSlidingExpiration(true);
        }

        private static bool HasModification(RequestContext context, DateTime modificationDate)
        {
            if (context.HttpContext.Request.Url == null || context.HttpContext.Request.Url.Host.Contains("localhost")) return true;
            var headerValue = context.HttpContext.Request.Headers["If-Modified-Since"];
            if (headerValue == null)
                return true;

            var modifiedSince = DateTime.Parse(headerValue).ToLocalTime();
            var diff = modifiedSince.Subtract(modificationDate);
            return diff < TimeSpan.FromSeconds(-1);
        }

        private static ActionResult NotModified(RequestContext response, DateTime lastModificationDate)
        {
            response.HttpContext.Response.Cache.SetLastModified(lastModificationDate);
            return new HttpStatusCodeResult(304, "Page has not been modified");
        }
    }
}