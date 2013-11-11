using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Web.Core;
using Blog.Web.Infrastructure;

namespace Blog.Web.Actions.PostGet
{
	public class PostGetModule : IModule
	{
		public void Execute(IContainer container)
		{
			container.Register(c => new PostGetController(c.Resolve<IMediator>()) {ActionInvoker = c.Resolve<IActionInvoker>()});

			var mediator = container.Resolve<ISubscribeHandlers>();

			mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
			{
				var result = new PostGetViewModel();
				result = new FilteredPostVault(container.Resolve<IReadOnlyList<PostViewModel>>())
								.Handle(message, result);
				return result;
			});

			mediator.Subscribe<PostIndexRequest, PostIndexViewModel>(message => new FilteredPostVault(container.Resolve<IReadOnlyList<PostViewModel>>()).Handle(message));
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
		public PostViewModel Post { get; set; }
		public PostViewModel Previous { get; set; }
		public PostViewModel Next { get; set; }
		
		public bool HasPrevious { get { return Previous != null; } }
		public bool HasNext { get { return Next != null; } }
	}

	public class PostIndexRequest { }

	public class PostIndexViewModel
	{
		public List<PostViewModel> Active { get; set; }
		public int FuturePostCount { get; set; }
	}
}
