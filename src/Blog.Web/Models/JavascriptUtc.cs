using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class JavascriptUtc : IPost
	{
		public string Title { get { return "Javascript UTC Datetime"; } }
		public string Slug { get { return "javascript-utc-datetime"; } }
		public string FileName { get { return "javascript-utc.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 6, 5, 10, 57, 0); } }
	}
}