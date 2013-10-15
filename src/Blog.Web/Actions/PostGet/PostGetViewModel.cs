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
		public IEnumerable<Post> Latest { get; private set; }
		public IReadOnlyCollection<Post> Future { get; set; }
		public int PostCount { get; private set; }

		public bool HasPrevious { get { return Previous != null; } }
		public bool HasNext { get { return Next != null; } }

		public PostGetViewModel(Post post, string content, Post previous, Post next,
								IEnumerable<Post> latest, IReadOnlyCollection<Post> future, int postCount)
		{
			Post = post;
			Content = content;
			Previous = previous;
			Next = next;
			Latest = latest;
			Future = future;
			PostCount = postCount;
		}
	}
}