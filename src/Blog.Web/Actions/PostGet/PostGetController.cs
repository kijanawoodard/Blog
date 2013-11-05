using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Blog.Web.Core;
using Blog.Web.Infrastructure;

namespace Blog.Web.Actions.PostGet
{
	public class PostGetModule : IModule
	{
		public void Execute(IContainer container)
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");

			container.Register(c => new PostGetController(c.Resolve<IMediator>()) {ActionInvoker = c.Resolve<IActionInvoker>()});

			var mediator = container.Resolve<ISubscribeHandlers>();
			mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
			{
				var result = new PostGetViewModel();
				result = new FilteredPostVault().Handle(message, result);
				result = new MarkdownContentStorage(root).Handle(message, result);
				return result;
			});

			mediator.Subscribe<PostIndexRequest, PostIndexViewModel>(message => new FilteredPostVault().Handle(message));
		}
	}

    public class PostGetController : Controller
    {
	    private readonly IMediator _mediator;

	    public PostGetController(IMediator mediator)
		{
		    _mediator = mediator;
		}

	    public ActionResult Execute(PostRequest request)
	    {
			var model = _mediator.Send<PostRequest, PostGetViewModel>(request);
			if (model.Post == null) return HttpNotFound();
			return View(model);
        }

		public ActionResult Index()
		{
			var model = _mediator.Send<PostIndexRequest, PostIndexViewModel>(new PostIndexRequest());
			return View(model);
		}
    }	

	public class PostRequest
	{
		public string Slug { get; set; }
	}

	public class PostGetViewModel
	{
		public Post Post { get; set; }
		public string Content { get; set; }
		public Post Previous { get; set; }
		public Post Next { get; set; }
		public IReadOnlyCollection<Post> Active { get; set; }
		public IReadOnlyCollection<Post> Future { get; set; }

		public bool HasPrevious { get { return Previous != null; } }
		public bool HasNext { get { return Next != null; } }
	}

	public class PostIndexRequest { }

	public class PostIndexViewModel
	{
		public IReadOnlyCollection<Post> Active { get; set; }
		public IReadOnlyCollection<Post> Future { get; set; }
	}
}
