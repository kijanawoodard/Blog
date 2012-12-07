using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Web.Core;

namespace Blog.Web.Infrastructure
{
	public class FilteredPostVault : IPostVault
	{
		public IReadOnlyList<IPost> Posts { get; private set; }

		public FilteredPostVault(IEnumerable<IPost> posts)
		{
			var now = DateTime.UtcNow;
			var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

			Posts = posts
				.OrderByDescending(x => x.PublishedAtCst)
				.Where(x => now >= TimeZoneInfo.ConvertTimeToUtc(x.PublishedAtCst, timezone))
				.ToList();
		}
	}
}