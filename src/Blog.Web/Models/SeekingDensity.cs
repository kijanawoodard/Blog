using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class SeekingDensity : IPost
	{
		public string Title { get { return "Seeking Density in Architectural Abstractions"; } }
		public string Slug { get { return "seeking-density-in-architectural-abstractions"; } }
		public string FileName { get { return "seeking-density.markdown"; } }
		public DateTime PublishedAtCst { get { return new DateTime(2012, 2, 14, 0, 0, 0); } }
	}
}