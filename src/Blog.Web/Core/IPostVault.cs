using System.Collections.Generic;

namespace Blog.Web.Core
{
	public interface IPostVault
	{
		IReadOnlyList<Post> Posts { get; }
		IReadOnlyList<Post> AllPosts { get; }
	}
}