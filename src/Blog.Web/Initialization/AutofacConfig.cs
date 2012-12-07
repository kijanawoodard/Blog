using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Blog.Web.Core;
using Blog.Web.Infrastructure;

namespace Blog.Web.Initialization
{
	public class AutofacConfig
	{
		public static void RegisterContainer()
		{
			var builder = new ContainerBuilder();
			var assembly = Assembly.GetExecutingAssembly();

			builder.RegisterControllers(assembly);
			builder.RegisterAssemblyTypes(assembly);
			builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces(); //http://code.google.com/p/autofac/wiki/Scanning
			builder.RegisterType<MarkdownContentStorage>()
			       .As<IContentStorage>()
			       .WithParameter("root", HttpContext.Current.Server.MapPath("~/Content/posts")); //do the httpcontext map here instead of buried in the class

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //mvc
		}
	}
}