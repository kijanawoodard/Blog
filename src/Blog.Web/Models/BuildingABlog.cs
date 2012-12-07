using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class BuildingABlog : IPost
	{
		public string Title { get { return "Building My Own Blog"; } }
		public string Slug { get { return "building-a-blog-engine"; } }
		public string FileName { get { return "building-blog.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 12, 6, 14, 0, 0); } }
	}

	public class JavascriptUtc : IPost
	{
		public string Title { get { return "Javascript UTC Datetime"; } }
		public string Slug { get { return "javascript-utc-datetime"; } }
		public string FileName { get { return "javascript-utc.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 6, 5, 10, 57, 0); } }
	}

	public class RavenId : IPost
	{
		public string Title { get { return "Just use string Id for RavenDB"; } }
		public string Slug { get { return "just-use-string-id-for-ravendb"; } }
		public string FileName { get { return "raven-id.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 5, 31, 0, 0, 0); } }
	}

	public class SeekingDensity : IPost
	{
		public string Title { get { return "Seeking Density in Architectural Abstractions"; } }
		public string Slug { get { return "seeking-density-in-architectural-abstractions"; } }
		public string FileName { get { return "seeking-density.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 2, 14, 0, 0, 0); } }
	}
}