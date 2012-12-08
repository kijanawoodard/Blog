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
			var post = _vault.Posts.FirstOrDefault();
			if (slug != null) post = _vault.Posts.FirstOrDefault(x => x.Slug.ToLower() == slug.ToLower());
			if (post == null) return HttpNotFound();

			var content = _storage.GetContent(post.FileName);

			var model = new PostGetViewModel(post, content, _vault.Posts.Take(30), _vault.Posts.Count);
            return View(model);
        }
    }
}
