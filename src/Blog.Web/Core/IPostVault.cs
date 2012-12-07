using System.Collections.Generic;

namespace Blog.Web.Core
{
	public interface IPostVault
	{
		IReadOnlyList<IPost> Posts { get; }
	}
}