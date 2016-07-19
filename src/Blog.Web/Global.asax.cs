using System;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.Infrastructure;
using Blog.Web.Initialization;

namespace Blog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DateTime LastModified;

        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            VesselConfig.RegisterContainer();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new AlternateViewEngine());

            LastModified = System.IO.File.GetLastWriteTime(Server.MapPath("Web.config"));
        }
    }
}