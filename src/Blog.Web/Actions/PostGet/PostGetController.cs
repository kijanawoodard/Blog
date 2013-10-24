using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Web.Core;
using Blog.Web.Infrastructure;

namespace Blog.Web.Actions.PostGet
{
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
}
