using System;

namespace Blog.Web.Core
{
	public interface IPost
	{
		string Title { get; }
		string Slug { get; }
		string FileName { get; }
		DateTime PublishedAtCst { get; }
	}
}