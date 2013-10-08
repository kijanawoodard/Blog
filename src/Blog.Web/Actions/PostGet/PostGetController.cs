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
			var post = _vault.Posts.First();
			if (slug != null) post = _vault.AllPosts.FirstOrDefault(x => x.Slug.ToLower() == slug.ToLower());
			if (post == null) return HttpNotFound();

			var content = _storage.GetContent(post.FileName);

		    var list = _vault.Posts.ToList(); //for indexOf
		    var count = _vault.Posts.Count;
		    var index = list.IndexOf(post);

		    var previous = index <= 0 ? null : list[index - 1];
			var next = index < 0 ? list[0] : 
					   index == count - 1 ? null : list[index + 1];

			var model = new PostGetViewModel(post, content, previous, next, _vault.Posts.Take(30), count);
            return View(model);
        }
    }
}
