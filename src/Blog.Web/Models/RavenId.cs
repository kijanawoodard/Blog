using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class RavenId : IPost
	{
		public string Title { get { return "Just use string Id for RavenDB"; } }
		public string Slug { get { return "just-use-string-id-for-ravendb"; } }
		public string FileName { get { return "raven-id.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 5, 31, 0, 0, 0); } }
	}
}