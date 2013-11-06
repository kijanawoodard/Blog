using System;
using System.Collections.Generic;
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
			if (context == null) return false;
			
			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return (request.AcceptTypes.Contains("application/atom+xml") 
					|| request.CurrentExecutionFilePathExtension.EndsWith("atom")
					|| (string)context.RouteData.Values["ext"] == "atom")
					&& ViewEngines.Engines.FindView(context, GetViewName(context), null).View != null;
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
				ViewName = GetViewName(context),
				ViewEngineCollection = viewResult.ViewEngineCollection,
			}; 
		}

		private string GetViewName(ControllerContext context)
		{
			return (string) context.RouteData.Values["action"] + ".atom";
		}
	}

	class JsonContentNegotiation : IHandleContentNegotiation
	{
		public bool CanHandle(ControllerContext context)
		{
			if (context == null) return false;
			
			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return request.AcceptTypes.Contains("application/json")
					|| request.CurrentExecutionFilePathExtension.EndsWith("json")
					|| (string)context.RouteData.Values["ext"] == "json";
		}

		public ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var model = context.Controller.ViewData.Model;

			if (model == null)
				return null;

			return new JsonNetResult { Data = model };
		}
	}

	class XmlContentNegotiation : IHandleContentNegotiation
	{
		public bool CanHandle(ControllerContext context)
		{
			if (context == null) return false;

			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return request.AcceptTypes.Contains("text/xml")
			         || request.CurrentExecutionFilePathExtension.EndsWith("xml")
			         || (string) context.RouteData.Values["ext"] == "xml";
		}

		public ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var model = context.Controller.ViewData.Model;

			if (model == null)
				return null;

			context.HttpContext.Response.ContentType = "text/xml";

			if (CustomViewExists(context))
				return new PartialViewResult
				{
					ViewData = context.Controller.ViewData,
					TempData = context.Controller.TempData,
					ViewName = GetViewName(context),
				}; 

			return new XmlResult(model);
		}

		private string GetViewName(ControllerContext context)
		{
			return (string)context.RouteData.Values["action"] + ".xml";
		}

		private bool CustomViewExists(ControllerContext context)
		{
			return ViewEngines.Engines.FindView(context, GetViewName(context), null).View != null;
		}
	}

	class CsvContentNegotiation : IHandleContentNegotiation
	{
		public bool CanHandle(ControllerContext context)
		{
			if (context == null) return false;

			var request = context.HttpContext.Request;
			if (request == null || request.AcceptTypes == null) return false;

			return (request.AcceptTypes.Contains("text/csv")
					|| request.CurrentExecutionFilePathExtension.EndsWith("csv")
					|| (string)context.RouteData.Values["ext"] == "csv");
		}

		public ActionResult Handle(ControllerContext context, ActionResult actionResult)
		{
			var model = context.Controller.ViewData.Model;

			if (model == null)
				return null;

			context.HttpContext.Response.ContentType = "text/csv";

			if (CustomViewExists(context))
				return new PartialViewResult
				{
					ViewData = context.Controller.ViewData,
					TempData = context.Controller.TempData,
					ViewName = GetViewName(context),
				};

			return new CsvResult(model);
		}

		private string GetViewName(ControllerContext context)
		{
			return (string)context.RouteData.Values["action"] + ".csv";
		}

		private bool CustomViewExists(ControllerContext context)
		{
			return ViewEngines.Engines.FindView(context, GetViewName(context), null).View != null;
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

			if (Data != null)
			{
				var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
				var serializer = JsonSerializer.Create(SerializerSettings);
				serializer.Serialize(writer, Data);

				writer.Flush();
			}
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

			//TODO: use the mediator to get csv friendly viewmodel - will be cleaner than relying on reflection 
			//At the moment - the class has to be "csv-able" to get any use here
			var writer = new CsvWriter(response.Output);
			
			var type = Data.GetType();
			var isEnumerable = type.GetInterfaces()
			                       .Any(i => i.IsGenericType &&
			                                 i.GetGenericTypeDefinition() == typeof (IEnumerable<>));
			if (isEnumerable)
			{
				//http://stackoverflow.com/a/4667999/214073
				//http://stackoverflow.com/a/1969419/214073
				//http://stackoverflow.com/a/906538/214073
				var method = typeof(EnumerableExtensions)
					.GetMethods(BindingFlags.Public | BindingFlags.Static)
					.Where(x => x.Name == "WriteCsv")
					.FirstOrDefault(x => x.GetParameters().Count() == 2);
				method = method.MakeGenericMethod(type.GetGenericArguments()[0]);
				method.Invoke(null, new object[]{Data, writer});
			}
			else //wrap ourselves
			{
				(new[] { Data }).WriteCsv(writer);	
			}
		}
	}
}

//http://www.mikesdotnetting.com/Article/80/Create-PDFs-in-ASP.NET-getting-started-with-iTextSharp
//http://lozanotek.com/blog/archive/2009/08/11/Inferred_Controller_Actions.aspx - similar to missing method; I don't think I want to do this

//PDF: https://github.com/webgio/Rotativa, http://pdfcrowd.com/html-to-pdf-api/, http://www.nyveldt.com/blog/post/Introducing-RazorPDF