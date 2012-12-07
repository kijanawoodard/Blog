using System.Collections.Generic;
using Blog.Web.Core;

namespace Blog.Web.Actions.PostGet
{
	public class PostGetViewModel
	{
		public IPost Post { get; private set; }
		public string Content { get; private set; }
		public IEnumerable<IPost> Latest { get; private set; }
		public int PostCount { get; private set; }

		public PostGetViewModel(IPost post, string content, IEnumerable<IPost> latest, int postCount)
		{
			Post = post;
			Content = content;
			Latest = latest;
			PostCount = postCount;
		}
	}
}