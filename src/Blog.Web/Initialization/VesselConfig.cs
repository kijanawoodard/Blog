using System.Web.Mvc;
using Blog.Web.Infrastructure;

namespace Blog.Web.Initialization
{
	public class VesselConfig
	{
		public static void RegisterContainer()
		{
			var mediator = new Mediator();
			
			var container = new Vessel();
			container.Register<IMediator>(mediator);
			container.Register<ISubscribeHandlers>(mediator);
			container.Register<IActionInvoker>(new PartialViewActionInvoker());
			container.RegisterModules();
			
			DependencyResolver.SetResolver(new VesselDependencyResolver(container)); //for asp.net mvc
		}
	}
}