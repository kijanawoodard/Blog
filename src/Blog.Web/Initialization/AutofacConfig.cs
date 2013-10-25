using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Blog.Web.Actions.AtomGet;
using Blog.Web.Actions.PostGet;
using Blog.Web.Infrastructure;

namespace Blog.Web.Initialization
{
	public class AutofacConfig
	{
		public static void RegisterContainer()
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");
			var assembly = Assembly.GetExecutingAssembly();
			
			var mediator = new Mediator();
			mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
			{
				var result = new PostGetViewModel();
				result = new FilteredPostVault().Handle(message, result);
				result = new MarkdownContentStorage(root).Handle(message, result);
				return result;
			});

			mediator.Subscribe<AtomRequest, AtomGetViewModel>(message => new FilteredPostVault().Handle(message));
			
			var builder = new ContainerBuilder();
			builder.RegisterControllers(assembly);
			builder.RegisterInstance<IMediator>(mediator);
			
			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //for asp.net mvc
		}
	}
}