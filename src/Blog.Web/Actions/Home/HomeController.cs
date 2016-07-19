using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Web.Actions.PostGet;
using Blog.Web.Infrastructure;

namespace Blog.Web.Actions.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            var model = _mediator.Send<PostIndexRequest, PostIndexViewModel>(new PostIndexRequest());
            return View(model);
        }
    }

    public class HomeModule : IModule
    {
        public void Execute(IContainer container)
        {
            container.Register(c => new HomeController(c.Resolve<IMediator>()) { ActionInvoker = c.Resolve<IActionInvoker>() });
        }
    }
}
