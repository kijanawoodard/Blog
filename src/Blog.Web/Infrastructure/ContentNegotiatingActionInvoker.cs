using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Xml.Serialization;
using Kent.Boogaart.KBCsv;
using Kent.Boogaart.KBCsv.Extensions;
using Newtonsoft.Json;

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

		protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
		{
			var customActionName =
				_handlers
					.Select(handler => handler.GetActionName(controllerContext))
					.FirstOrDefault(x => x != null);

			if (customActionName == null)
				return base.FindAction(controllerContext, controllerDescriptor, actionName);

			return base.FindAction(controllerContext, controllerDescriptor, customActionName)
						 ?? base.FindAction(controllerContext, controllerDescriptor, actionName); //this would be a lot nicer if FindAction just returned null if the input is null or empty
		}

		protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
		{
			if (actionReturnValue == null)
				return new HttpNotFoundResult(); //not entirely sure this is a good choice universally, but trying it on for size

			if (actionReturnValue is ActionResult)
				return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);

			controllerContext.Controller.ViewData.Model = actionReturnValue;
			return new ViewResult
			{
				ViewData = controllerContext.Controller.ViewData,
				TempData = controllerContext.Controller.TempData,
				ViewName = actionDescriptor.ActionName,
			};
		}
		
		protected override ActionResult InvokeActionMethod(ControllerContext controllerContext,
														   ActionDescriptor actionDescriptor,
														   IDictionary<string, object> parameters)
		{
			var baseResult = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
			var result = _handlers.Select(handler => handler.Handle(controllerContext, baseResult)).FirstOrDefault(x => x != null);
			return result ?? baseResult;
		}
	}

	//http://www.strathweb.com/2012/07/everything-you-want-to-know-about-asp-net-web-api-content-negotation/
	//identify media type
 	//look for custom action
	//look for custom action-less view
	//return default formatter

	public interface IHandleContentNegotiation
	{
		ActionResult Handle(ControllerContext context, ActionResult actionResult);
		string GetActionName(ControllerContext context);
	}

	//having a bit of fun with CPS
	abstract class ContentNegotiationBase : IHandleContentNegotiation
	{
		protected ICollection<string> MediaTypes { get; private set; }
		protected string Extension { get; set; }

		protected ContentNegotiationBase()
		{
			MediaTypes = new Collection<string>();
		}

		protected bool SupportsMediaTypes(ControllerContext context)
		{
			if (context == null) return false;
			
			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return MediaTypes.Any(x => request.AcceptTypes.Contains(x));
		}

		protected bool SupportsExtensions(ControllerContext context)
		{
			if (context == null) return false;

			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return request.CurrentExecutionFilePathExtension.EndsWith(Extension)
					|| (string)context.RouteData.Values["ext"] == Extension;
		}

		protected string GetCustomViewName(ControllerContext context)
		{
			return (string)context.RouteData.Values["action"] + "." + Extension;
		}

		protected bool CustomViewExists(ControllerContext context)
		{
			return ViewEngines.Engines.FindView(context, GetCustomViewName(context), null).View != null;
		}

		protected virtual bool CanHandle(ControllerContext context)
		{
			return SupportsMediaTypes(context) || SupportsExtensions(context);
		}

		public abstract ActionResult Handle(ControllerContext context, ActionResult actionResult);

		public virtual string GetActionName(ControllerContext context)
		{
			return CanHandle(context) ? Extension : null;
		}
	}

	class PartialViewNegotiation : ContentNegotiationBase
	{
		public PartialViewNegotiation()
		{
			MediaTypes.Add("text/html");
			Extension = "phtml";
		}

		protected override bool CanHandle(ControllerContext context)
		{
			return SupportsExtensions(context)
			       || (SupportsMediaTypes(context) && context.IsChildAction);
		}

		public override ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			if (!CanHandle(context)) return null;

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

	class AtomContentNegotiation : ContentNegotiationBase
	{
		public AtomContentNegotiation()
		{
			MediaTypes.Add("application/atom+xml");
			Extension = "atom";
		}

		public override ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var ok = CanHandle(context) && CustomViewExists(context);
			if (!ok) return null;

			var viewResult = actionResult as ViewResult;
			if (viewResult == null) return null;

			context.HttpContext.Response.ContentType = "application/atom+xml";

			return new PartialViewResult
			{
				ViewData = viewResult.ViewData,
				TempData = viewResult.TempData,
				ViewName = GetCustomViewName(context),
				ViewEngineCollection = viewResult.ViewEngineCollection,
			}; 
		}
	}

	class JsonContentNegotiation : ContentNegotiationBase
	{
		public JsonContentNegotiation()
		{
			MediaTypes.Add("application/json");
			Extension = "json";
		}
		
		public override ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			if (!CanHandle(context)) return null;
			var model = context.Controller.ViewData.Model;
			return model == null ? null : new JsonNetResult { Data = model };
		}
	}

	class XmlContentNegotiation : ContentNegotiationBase
	{
		public XmlContentNegotiation()
		{
			MediaTypes.Add("text/xml");
			Extension = "xml";
		}
		
		public override ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			if (!CanHandle(context)) return null;
			var model = context.Controller.ViewData.Model;
			if (model == null) return null;

			context.HttpContext.Response.ContentType = "text/xml";

			if (CustomViewExists(context))
				return new PartialViewResult
				{
					ViewData = context.Controller.ViewData,
					TempData = context.Controller.TempData,
					ViewName = GetCustomViewName(context),
				}; 

			return new XmlResult(model);
		}
	}

	class CsvContentNegotiation : ContentNegotiationBase
	{
		public CsvContentNegotiation()
		{
			MediaTypes.Add("text/csv");
			Extension = "csv";
		}

		public override ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			if (!CanHandle(context)) return null;
			var model = context.Controller.ViewData.Model;
			if (model == null) return null;

			context.HttpContext.Response.ContentType = "text/csv";

			if (CustomViewExists(context))
				return new PartialViewResult
				{
					ViewData = context.Controller.ViewData,
					TempData = context.Controller.TempData,
					ViewName = GetCustomViewName(context),
				};

			return new CsvResult(model);
		}
	}

	public class JsonNetResult : ActionResult
	{
		public Encoding ContentEncoding { get; set; }
		public string ContentType { get; set; }
		public object Data { get; set; }

		public JsonSerializerSettings SerializerSettings { get; set; }
		public Formatting Formatting { get; set; }

		public JsonNetResult()
		{
			SerializerSettings = new JsonSerializerSettings();
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			var response = context.HttpContext.Response;

			response.ContentType = !string.IsNullOrEmpty(ContentType)
			  ? ContentType
			  : "application/json";

			if (ContentEncoding != null)
				response.ContentEncoding = ContentEncoding;

			if (Data == null) return;
			var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
			var serializer = JsonSerializer.Create(SerializerSettings);
			serializer.Serialize(writer, Data);

			writer.Flush();
		}
	}

	public class XmlResult : ActionResult
	{
		public object Data { get; private set; }
		public string ContentType { get; set; }
		public Encoding Encoding { get; set; }

		public XmlResult(object data)
		{
			if (data == null) throw new ArgumentNullException("data");

			Data = data;
			ContentType = "text/xml";
			Encoding = Encoding.UTF8;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.HttpContext.Response;

			response.ContentType = ContentType;
			response.HeaderEncoding = Encoding;

			var serializer = new XmlSerializer(Data.GetType());
			serializer.Serialize(response.Output, Data);
		}
	}

	class CsvResult : ActionResult
	{
		public object Data { get; private set; }
		public string ContentType { get; set; }
		public Encoding Encoding { get; set; }

		public CsvResult(object data)
		{
			if (data == null) throw new ArgumentNullException("data");

			Data = data;
			ContentType = "text/csv";
			Encoding = Encoding.UTF8;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var response = context.HttpContext.Response;

			response.ContentType = ContentType;
			response.HeaderEncoding = Encoding;

			var writer = new CsvWriter(response.Output);
			
			var type = Data.GetType();
			var isEnumerable = type.GetInterfaces()
			                       .Any(i => i.IsGenericType &&
			                                 i.GetGenericTypeDefinition() == typeof (IEnumerable<>));
			if (!isEnumerable)
			{
				var arr = Array.CreateInstance(type, 1);
				arr.SetValue(Data, 0); //wrap ourselves - anonymous arrays don't render
				Data = arr; 
				type = Data.GetType();
			}

			//http://stackoverflow.com/a/4667999/214073
			//http://stackoverflow.com/a/1969419/214073
			//http://stackoverflow.com/a/906538/214073
			var method = typeof(EnumerableExtensions)
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.Where(x => x.Name == "WriteCsv")
				.First(x => x.GetParameters().Count() == 2);

			Type typeToMake;
			if (type.IsArray)
				typeToMake = type.GetElementType();
			else if (type.IsGenericType)
				typeToMake = type.GetGenericArguments()[0];
			else
				throw new ArgumentException("Wasn't expecting to be here");

			method = method.MakeGenericMethod(typeToMake);
			method.Invoke(null, new object[] { Data, writer });
		}
	}
}

//http://www.mikesdotnetting.com/Article/80/Create-PDFs-in-ASP.NET-getting-started-with-iTextSharp
//http://lozanotek.com/blog/archive/2009/08/11/Inferred_Controller_Actions.aspx - similar to missing method; I don't think I want to do this

//PDF: https://github.com/webgio/Rotativa, http://pdfcrowd.com/html-to-pdf-api/, http://www.nyveldt.com/blog/post/Introducing-RazorPDF