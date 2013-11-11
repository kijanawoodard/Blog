using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Web.Content;
using Blog.Web.Core;
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
						new JsonContentNegotiation(), 
						new XmlContentNegotiation(), 
						new CsvContentNegotiation(), 
					});

			container.Register<IActionInvoker>(invoker);
		}
	}

	public class PostsModule : IModule
	{
		public void Execute(IContainer container)
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");
			var posts =
				WritingPosts.Posts
							.Select(post => new PostViewModel
							{
								Title = post.Title,
								Slug = post.Slug,
								FileName = post.FileName,
								PublishedAtCst = post.PublishedAtCst
							})
							.Select(vm => new MarkdownContentStorage(root).Handle(vm))
							.ToList();

			container.Register<IReadOnlyList<PostViewModel>>(posts);
		}
	}
}