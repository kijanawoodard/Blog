using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.Infrastructure;
using Blog.Web.Initialization;

namespace Blog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            VesselConfig.RegisterContainer();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new AlternateViewEngine());
        }
    }
}