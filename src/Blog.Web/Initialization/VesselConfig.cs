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
			container.RegisterModules();
			
			DependencyResolver.SetResolver(new VesselDependencyResolver(container)); //for asp.net mvc
		}
	}

	public class ActionInvokerModule : IModule
	{
		public void Execute(IContainer container)
		{
			var invoker = 
				new ContentNegotiatingActionInvoker(
					new IHandleContentNegotiation[]
					{
						new PartialViewNegotiation(),
						new AtomContentNegotiation(),
					});

			container.Register<IActionInvoker>(invoker);
		}
	}
}