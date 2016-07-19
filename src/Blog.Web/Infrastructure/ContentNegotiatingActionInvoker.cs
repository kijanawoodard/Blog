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
            var result = _handlers.Select(handler => handler.Handle(controllerContext)).FirstOrDefault(x => x != null);
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
        ActionResult Handle(ControllerContext context);
        string GetActionName(ControllerContext context);
    }

    //having a bit of fun with CPS
    abstract class ContentNegotiationBase : IHandleContentNegotiation
    {
        protected ICollection<string> MediaTypes { get; private set; }
        protected string Extension { get; set; }
        protected bool AlwaysReturnPartial { get; set; }

        protected ContentNegotiationBase()
        {
            MediaTypes = new Collection<string>();
        }

        protected bool SupportsMediaTypes(ControllerContext context)
        {
            if (context == null) return false;
            
            var request = context.HttpContext.Request;
            if (request == null || request.AcceptTypes == null) return false;

            return MediaTypes.Any(x => request.AcceptTypes.Contains(x) || request.ContentType == x);
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

        /// <summary>
        /// override to affect both GetActionName and Handle
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool CanHandle(ControllerContext context)
        {
            var ok = SupportsMediaTypes(context) || SupportsExtensions(context);
            return ok && OnCanHandle(context);
        }

        /// <summary>
        ///  override for cases where you only want to affect the CanHandle call within Handle
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool OnCanHandle(ControllerContext context)
        {
            return true; 
        }

        public ActionResult Handle(ControllerContext context)
        {
            if (!CanHandle(context)) return null;
            var model = context.Controller.ViewData.Model;
            if (model == null) return null;

            context.HttpContext.Response.ContentType = MediaTypes.First();
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;

            if (AlwaysReturnPartial || CustomViewExists(context))
                return new PartialViewResult
                {
                    ViewData = context.Controller.ViewData,
                    TempData = context.Controller.TempData,
                    ViewName = CustomViewExists(context) ? GetCustomViewName(context) : (string)context.RouteData.Values["action"],
                };

            return OnHandle(context);
        }

        protected virtual ActionResult OnHandle(ControllerContext context)
        {
            return null;
        }

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
            AlwaysReturnPartial = true;
        }

        protected override bool CanHandle(ControllerContext context)
        {
            return SupportsExtensions(context)
                   || (SupportsMediaTypes(context) && context.IsChildAction);
        }
    }

    class AtomContentNegotiation : ContentNegotiationBase
    {
        public AtomContentNegotiation()
        {
            MediaTypes.Add("application/atom+xml");
            Extension = "atom";
        }

        protected override bool OnCanHandle(ControllerContext context)
        {
            return CustomViewExists(context);
        }
    }

    class JsonContentNegotiation : ContentNegotiationBase
    {
        public JsonContentNegotiation()
        {
            MediaTypes.Add("application/json");
            Extension = "json";
        }
        
        protected override ActionResult OnHandle(ControllerContext context)
        {
            return new JsonNetResult(context.Controller.ViewData.Model);
        }
    }

    class XmlContentNegotiation : ContentNegotiationBase
    {
        public XmlContentNegotiation()
        {
            MediaTypes.Add("text/xml");
            Extension = "xml";
        }
        
        protected override ActionResult OnHandle(ControllerContext context)
        {
            return new XmlResult(context.Controller.ViewData.Model);
        }
    }

    class CsvContentNegotiation : ContentNegotiationBase
    {
        public CsvContentNegotiation()
        {
            MediaTypes.Add("text/csv");
            Extension = "csv";
        }

        protected override ActionResult OnHandle(ControllerContext context)
        {
            return new CsvResult(context.Controller.ViewData.Model);
        }
    }

    public class JsonNetResult : ActionResult
    {
        private readonly object _data;

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult(object data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _data = data;
            
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            var serializer = JsonSerializer.Create(SerializerSettings);
            
            serializer.Serialize(writer, _data);
            writer.Flush();
        }
    }

    public class XmlResult : ActionResult
    {
        private readonly object _data;

        public XmlResult(object data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var serializer = new XmlSerializer(_data.GetType());
            serializer.Serialize(response.Output, _data);
        }
    }

    class CsvResult : ActionResult
    {
        private object _data;

        public CsvResult(object data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var writer = new CsvWriter(response.Output);

            var type = _data.GetType();
            var isEnumerable = type.GetInterfaces()
                                   .Any(i => i.IsGenericType &&
                                             i.GetGenericTypeDefinition() == typeof (IEnumerable<>));
            if (!isEnumerable)
            {
                var arr = Array.CreateInstance(type, 1);
                arr.SetValue(_data, 0); //wrap ourselves - anonymous arrays don't render
                _data = arr;
                type = _data.GetType();
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
            method.Invoke(null, new object[] { _data, writer });
        }
    }
}

//http://www.mikesdotnetting.com/Article/80/Create-PDFs-in-ASP.NET-getting-started-with-iTextSharp
//http://lozanotek.com/blog/archive/2009/08/11/Inferred_Controller_Actions.aspx - similar to missing method; I don't think I want to do this

//PDF: https://github.com/webgio/Rotativa, http://pdfcrowd.com/html-to-pdf-api/, http://www.nyveldt.com/blog/post/Introducing-RazorPDF