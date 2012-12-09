using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Web.Initialization
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				null,
				"atom",
				new { controller = "AtomGet", action = "Execute" });

			routes.MapRoute(
				null,
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