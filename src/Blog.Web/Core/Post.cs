using System;

namespace Blog.Web.Core
{
	public class Post
	{
		public string Title { get; set; }
		public string Slug { get; set; }
		public string FileName { get; set; }
		public DateTime PublishedAtCst { get; set; }
		public string[] Tags { get; set; }

		public Post()
		{
			Tags = new string[] {};
		}
	}

	public class PostViewModel
	{
		public string Title { get; set; }
		public string Slug { get; set; }
		public string FileName { get; set; }
		public DateTime PublishedAtCst { get; set; }
		
		public string Summary
		{
			get
			{
				if (Content == null) return Title;
				var firstParagraph = Content.IndexOf("\n", System.StringComparison.Ordinal);
				if (firstParagraph == -1) return Title;

				var secondParagraph = Content.IndexOf("\n", firstParagraph + 2, System.StringComparison.Ordinal);
				return Content.Substring(0, secondParagraph == -1 ? firstParagraph : secondParagraph);
			}
		}

		public string Content { get; set; }
	}
}