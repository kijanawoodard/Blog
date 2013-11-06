using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Blog.Web.Actions.DisplayErrors;
using Blog.Web.Infrastructure;
using Blog.Web.Initialization;

namespace Blog.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			VesselConfig.RegisterContainer();

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new AlternateViewEngine());
		}

		//http://stackoverflow.com/a/9026907/214073
//		protected void Application_EndRequest()
//		{
//			if (Context.Response.StatusCode == 404)
//			{
//				Response.Clear();
//
//				var rd = new RouteData();
//				rd.Values["controller"] = "DisplayErrors";
//				rd.Values["action"] = "Http404";
//
//				IController c = new DisplayErrorsController();
//				c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
//			}
//		}
	}
}