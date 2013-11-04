using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Blog.Web.Infrastructure
{
	//http://stackoverflow.com/questions/7109967/using-json-net-as-default-json-serializer-in-asp-net-mvc-3-is-it-possible/11878694#11878694
	//http://lozanotek.com/blog/archive/2010/10/06/poco_results_for_mvc_actions.aspx
	//http://ben.onfabrik.com/posts/content-negotiation-in-aspnet-mvc

	public class PartialViewActionInvoker : ControllerActionInvoker
	{
		protected override ActionResult InvokeActionMethod(ControllerContext controllerContext,
		                                                   ActionDescriptor actionDescriptor,
		                                                   IDictionary<string, object> parameters)
		{
			var actionResult = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);

			if (IsSatisfiedBy(controllerContext))
			{
				var viewResult = actionResult as ViewResult;

				if (viewResult == null)
					return null;

				var partialViewResult = new PartialViewResult
				{
					ViewData = viewResult.ViewData,
					TempData = viewResult.TempData,
					ViewName = viewResult.ViewName,
					ViewEngineCollection = viewResult.ViewEngineCollection,
				};

				return partialViewResult;
			}

			return actionResult;
		}

		public bool IsSatisfiedBy(ControllerContext controllerContext)
		{
			return controllerContext.HttpContext.Request.AcceptTypes.Contains("text/html")
			       && (controllerContext.HttpContext.Request.IsAjaxRequest() || controllerContext.IsChildAction);
		}
	}
}