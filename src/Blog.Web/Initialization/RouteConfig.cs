using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.Initialization
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Error - 404",
                "NotFound",
                new { controller = "DisplayErrors", action = "Http404" }
                );

            //            routes.MapRoute(
            //                "Error - 500",
            //                "ServerError",
            //                new { controller = "DisplayErrors", action = "Http500" }
            //                );

            routes.MapRoute(
                null,
                "sitemap",
                new { controller = "Sitemap", action = "Index" });

            routes.MapRoute(
                null,
                "archive.{ext}",
                new { controller = "PostGet", action = "Index" });

            routes.MapRoute(
                null,
                "archive",
                new { controller = "PostGet", action = "Index" });

            routes.MapRoute(
                null,
                "about",
                new { controller = "AboutGet", action = "Index" });

            routes.MapRoute(
                "Root",
                "",
                new {controller = "Home", action = "Index", id = ""}
                );

            routes.MapRoute(
                null,
                "{slug}.{ext}",
                new { controller = "PostGet", action = "Execute" });

            routes.MapRoute(
                "canonical-slug",
                "{slug}",
                new {controller = "PostGet", action = "Execute"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PostGet", action = "Execute", id = UrlParameter.Optional }
            );
        }
    }
}