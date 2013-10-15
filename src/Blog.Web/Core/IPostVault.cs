using System.Collections.Generic;

namespace Blog.Web.Core
{
	public interface IPostVault
	{
		IReadOnlyList<Post> ActivePosts { get; }
		IReadOnlyList<Post> FuturePosts { get; }
		IReadOnlyList<Post> AllPosts { get; }
	}
}