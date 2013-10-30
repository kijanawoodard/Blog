using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Web.Core;
using Blog.Web.Infrastructure;

namespace Blog.Web.Actions.AtomGet
{
    public class AtomGetController : Controller
    {
	    private readonly IMediator _mediator;

	    public AtomGetController(IMediator mediator)
	    {
		    _mediator = mediator;
	    }

	    public ActionResult Execute()
	    {
            Response.ContentType = "application/atom+xml";
			var model = _mediator.Send<AtomRequest, AtomGetViewModel>(new AtomRequest());
            return View(model);
        }
    }

	public class AtomRequest { }
	public class AtomGetViewModel
	{
		public IReadOnlyList<Post> Posts { get; set; } 
	}
}
