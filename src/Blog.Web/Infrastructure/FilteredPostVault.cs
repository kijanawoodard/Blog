using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Web.Actions.PostGet;
using Blog.Web.Content;
using Blog.Web.Core;

namespace Blog.Web.Infrastructure
{
	public class PostGetModule : IModule
	{
		public void Execute(IContainer container)
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");
			var posts =
				WritingPosts.Posts
				            .Select(post => new PostViewModel
				            {
					            Title = post.Title,
					            Slug = post.Slug,
					            FileName = post.FileName,
					            PublishedAtCst = post.PublishedAtCst
				            })
							.Select(vm => new MarkdownContentStorage(root).Handle(vm))
							.ToList() as IReadOnlyList<PostViewModel>;

			container.Register(posts);
		}
	}

	public class FilteredPostVault
	{
		private IReadOnlyCollection<PostViewModel> ActivePosts { get; set; }
		private IReadOnlyCollection<PostViewModel> FuturePosts { get; set; }
		private IReadOnlyCollection<PostViewModel> AllPosts { get; set; }

		public FilteredPostVault(IReadOnlyCollection<PostViewModel> posts)
		{
			var now = DateTime.UtcNow;
			var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

			AllPosts = posts;

			ActivePosts =
				AllPosts
					.OrderByDescending(x => x.PublishedAtCst)
					.Where(x => now >= TimeZoneInfo.ConvertTimeToUtc(x.PublishedAtCst, timezone))
					.ToList();

			FuturePosts = AllPosts.Except(ActivePosts).ToList();
		}

		public PostGetViewModel Handle(PostRequest message, PostGetViewModel result)
		{
			var post = ActivePosts.FirstOrDefault();
			if (message.Slug != null) post = AllPosts.FirstOrDefault(x => x.Slug.ToLower() == message.Slug.ToLower());
			if (post == null) return result; //Decision: don't throw, handle downstream as to what this means

			var previous = ActivePosts.OrderBy(x => x.PublishedAtCst).FirstOrDefault(x => x.PublishedAtCst > post.PublishedAtCst);
			var next = ActivePosts.FirstOrDefault(x => x.PublishedAtCst < post.PublishedAtCst);

			result.Post = post;
			result.Previous = previous;
			result.Next = next;

			return result;
		}

		public PostIndexViewModel Handle(PostIndexRequest message)
		{
			var result = new PostIndexViewModel();
			result.Active = ActivePosts.ToList();
			result.FuturePostCount = FuturePosts.Count;

			return result;
		}
	}
}