using System.Web;
using System.Web.Mvc;
using Blog.Web.Actions.AtomGet;
using Blog.Web.Actions.PostGet;
using Blog.Web.Infrastructure;

namespace Blog.Web.Initialization
{
	public class VesselConfig
	{
		public static void RegisterContainer()
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");

			var mediator = new Mediator();
			mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
			{
				var result = new PostGetViewModel();
				result = new FilteredPostVault().Handle(message, result);
				result = new MarkdownContentStorage(root).Handle(message, result);
				return result;
			});

			mediator.Subscribe<AtomRequest, AtomGetViewModel>(message => new FilteredPostVault().Handle(message));

			var container = new Vessel();
			container.Register<IMediator>(mediator);

			container.Register(c => new PostGetController(c.Resolve<IMediator>()));
			container.Register(c => new AtomGetController(c.Resolve<IMediator>()));

			DependencyResolver.SetResolver(new VesselDependencyResolver(container)); //for asp.net mvc
		}
	}
}