using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Optimization;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            VesselConfig.RegisterContainer();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new AlternateViewEngine());

            LastModified = System.IO.File.GetLastWriteTime(Server.MapPath("Web.config"));
        }
    }
}