using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Blog.Web.Infrastructure
{
	//http://stackoverflow.com/questions/7109967/using-json-net-as-default-json-serializer-in-asp-net-mvc-3-is-it-possible/11878694#11878694
	//http://lozanotek.com/blog/archive/2010/10/06/poco_results_for_mvc_actions.aspx
	//http://ben.onfabrik.com/posts/content-negotiation-in-aspnet-mvc
	//http://lozanotek.com/blog/archive/2010/10/06/poco_results_for_mvc_actions.aspx
	public class ContentNegotiatingActionInvoker : ControllerActionInvoker
	{
		private readonly IHandleContentNegotiation[] _handlers;

		public ContentNegotiatingActionInvoker(IHandleContentNegotiation[] handlers)
		{
			_handlers = handlers;
		}

		protected override ActionResult InvokeActionMethod(ControllerContext controllerContext,
														   ActionDescriptor actionDescriptor,
														   IDictionary<string, object> parameters)
		{
			var baseResult = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);

			foreach (var handler in _handlers)
			{
				if (handler.CanHandle(controllerContext))
					return handler.Handle(controllerContext, baseResult);
			}

			return baseResult;
		}
	}

	public interface IHandleContentNegotiation
	{
		bool CanHandle(ControllerContext context);
		ActionResult Handle(ControllerContext context, ActionResult actionResult);
	}

	class PartialViewNegotiation : IHandleContentNegotiation
	{
		public bool CanHandle(ControllerContext context)
		{
			if (context == null || context.HttpContext.Request == null || context.HttpContext.Request.AcceptTypes == null) return false;
			return context.HttpContext.Request.AcceptTypes.Contains("text/html")
				   && (context.HttpContext.Request.IsAjaxRequest() || context.IsChildAction); //cargo cult-ing IsAjaxRequest. I understand it, but I think that case may need more attention
		}

		public ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var viewResult = actionResult as ViewResult;
			if (viewResult == null) return null;

			return new PartialViewResult
			{
				ViewData = viewResult.ViewData,
				TempData = viewResult.TempData,
				ViewName = viewResult.ViewName,
				ViewEngineCollection = viewResult.ViewEngineCollection,
			};
		}
	}

	class AtomContentNegotiation : IHandleContentNegotiation
	{
		public bool CanHandle(ControllerContext context)
		{
			if (context == null || context.HttpContext.Request == null || context.HttpContext.Request.AcceptTypes == null) return false;
			return context.HttpContext.Request.AcceptTypes.Contains("application/atom+xml");
		}

		public ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var viewResult = actionResult as ViewResult;
			if (viewResult == null) return null;

			context.HttpContext.Response.ContentType = "application/atom+xml";

			return new PartialViewResult
			{
				ViewData = viewResult.ViewData,
				TempData = viewResult.TempData,
				ViewName = "atom",
				ViewEngineCollection = viewResult.ViewEngineCollection,
			}; 
		}
	}
}

//http://www.mikesdotnetting.com/Article/80/Create-PDFs-in-ASP.NET-getting-started-with-iTextSharp
//http://lozanotek.com/blog/archive/2009/08/11/Inferred_Controller_Actions.aspx - similar to missing method; I don't think I want to do this