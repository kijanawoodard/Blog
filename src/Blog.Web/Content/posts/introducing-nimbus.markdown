Once I started [questioning IoC] containers, a [variety][violating isp] of [problems][violating srp] presented [themselves][foo ifoo].

Near the end of [questioning IoC], I posited an escape hatch:

		new Mediator(new DoThis(), new DoThat(), new DoTheOther());

Working to achieve this api, I came up with [Nimbus].

To see how it works, I incorporated nimbus to run this blog.

Here is the controller that displays this page [before]:

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
			var posts = _vault.ActivePosts; 
			var post = posts.FirstOrDefault();
			if (slug != null) post = _vault.AllPosts.FirstOrDefault(x => x.Slug.ToLower() == slug.ToLower());
			if (post == null) return HttpNotFound();

			var content = _storage.GetContent(post.FileName);
		    var previous = posts.OrderBy(x => x.PublishedAtCst).FirstOrDefault(x => x.PublishedAtCst > post.PublishedAtCst);
			var next = posts.FirstOrDefault(x => x.PublishedAtCst < post.PublishedAtCst);

			var model = new PostGetViewModel(post, content, previous, next, _vault.ActivePosts, _vault.FuturePosts);
            return View(model);
        }
    }

Here is the controller that displays this page [after]:

    public class PostGetController : Controller
    {
	    private readonly IMediator _mediator;

	    public PostGetController(IMediator mediator)
		{
		    _mediator = mediator;
		}

	    public ActionResult Execute(PostRequest request)
	    {
			var model = _mediator.Send<PostRequest, PostGetViewModel>(request);
			if (model.Post == null) return HttpNotFound();
			return View(model);
        }
    }		

The [135 loc source file][nimbus source] is meant for copy/paste inclusion and modification. For instance, say you want to decorate each handler instance with logging/timing/etc. Salt to taste.

It was an interesting journey that led to [other realizations][partial application]. I also noticed how tempting it is to add features. 

What kept me in check was [ISP]. I thought [SRP] was a better check, but I kept be able to justify responsibility. Yeah, that feature is close enough. We don't need an entirely different class for just this.

It turned out the ISP kept me on the straight and narrow. After ranting about [violating isp], I could hardly force clients to take dependencies they weren't going to use. That led me to write a bunch of code to cover the signature permutations. The bloat then led me to cut features that weren't truly pertinent. 

[questioning ioc]: /questioning-ioc-containers
[violating isp]: /violating-isp-with-constructor-injection
[violating srp]: /violating-srp-with-constructor-injection
[foo ifoo]: /foo-ifoo-is-an-anti-pattern
[Nimbus]: https://github.com/kijanawoodard/nimbus
[nimbus source]: https://github.com/kijanawoodard/nimbus/blob/b594b02a5770bf142b19f1ab468967d5f0bab694/src/mediator.cs
[partial application]: /constructor-injection-is-partial-application
[isp]: http://en.wikipedia.org/wiki/Interface_segregation_principle
[srp]: http://en.wikipedia.org/wiki/Single_responsibility_principle
[before]: https://github.com/kijanawoodard/Blog/blob/300ecdf6b48190849b204dbf0ad20b5c80dfd4f4/src/Blog.Web/Actions/PostGet/PostGetController.cs