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
		public string Content { get; set; }
	}
}