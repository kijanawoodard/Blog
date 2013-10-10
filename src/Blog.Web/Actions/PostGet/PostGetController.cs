using System.Linq;
using System.Web.Mvc;
using Blog.Web.Core;

namespace Blog.Web.Actions.PostGet
{
    public class PostGetController : Controller
    {
		private readonly IPostVault _vault;
	    private readonly IContentStorage _storage;

	    public PostGetController(IPostVault vault, IContentStorage storage)
		{
			_vault = vault;
			_storage = storage;
		}

	    public ActionResult Execute(string slug)
	    {
			var posts = _vault.Posts.ToList(); 
			var post = posts.FirstOrDefault();
			if (slug != null) post = _vault.AllPosts.FirstOrDefault(x => x.Slug.ToLower() == slug.ToLower());
			if (post == null) return HttpNotFound();

			var content = _storage.GetContent(post.FileName);
		    var previous = posts.OrderBy(x => x.PublishedAtCst).FirstOrDefault(x => x.PublishedAtCst > post.PublishedAtCst);
			var next = posts.FirstOrDefault(x => x.PublishedAtCst < post.PublishedAtCst);

		    var future = _vault.AllPosts.Except(posts).ToList();
			var model = new PostGetViewModel(post, content, previous, next, _vault.Posts, future, posts.Count);
            return View(model);
        }
    }
}
