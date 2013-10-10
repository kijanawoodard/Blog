using System.Collections.Generic;
using Blog.Web.Core;

namespace Blog.Web.Actions.PostGet
{
	public class PostGetViewModel
	{
		public IPost Post { get; private set; }
		public string Content { get; private set; }
		public IPost Previous { get; private set; }
		public IPost Next { get; private set; }
		public IEnumerable<IPost> Latest { get; private set; }
		public IReadOnlyCollection<IPost> Future { get; set; }
		public int PostCount { get; private set; }

		public bool HasPrevious { get { return Previous != null; } }
		public bool HasNext { get { return Next != null; } }

		public PostGetViewModel(IPost post, string content, IPost previous, IPost next,
								IEnumerable<IPost> latest, IReadOnlyCollection<IPost> future, int postCount)
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