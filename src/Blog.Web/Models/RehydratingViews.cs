using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class RehydratingViews : IPost
	{
		public string Title { get { return "FubuMVC, Validation, and Re-Hydrating the View"; } }
		public string Slug { get { return "fubumvc-validation-and-re-hydrating-the-view"; } }
		public string FileName { get { return "rehydrating-views.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("February 12, 2012"); } }
	}
}