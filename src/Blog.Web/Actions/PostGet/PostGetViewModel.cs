using System.Collections.Generic;
using Blog.Web.Core;

namespace Blog.Web.Actions.PostGet
{
	public class PostGetViewModel
	{
		public Post Post { get; private set; }
		public string Content { get; private set; }
		public Post Previous { get; private set; }
		public Post Next { get; private set; }
		public IReadOnlyCollection<Post> Active { get; private set; }
		public IReadOnlyCollection<Post> Future { get; set; }

		public bool HasPrevious { get { return Previous != null; } }
		public bool HasNext { get { return Next != null; } }

		public PostGetViewModel(Post post, string content, Post previous, Post next,
								IReadOnlyCollection<Post> latest, IReadOnlyCollection<Post> future)
		{
			Post = post;
			Content = content;
			Previous = previous;
			Next = next;
			Active = latest;
			Future = future;
		}
	}
}