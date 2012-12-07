using System.Web.Mvc;

namespace Blog.Web.Infrastructure
{
	public class AlternateViewEngine : RazorViewEngine
	{
		public AlternateViewEngine()
		{
			ViewLocationFormats = new[] { "~/Actions/{1}/{0}.cshtml" };
		}
	}
}