---
title: Introducing Nimbus
published: October 24, 2013
tags: 
---

Once I started [questioning IoC] containers, a [variety][violating isp] of [problems][violating srp] presented [themselves][foo ifoo].

Near the end of [questioning IoC], I posited an escape hatch:

        new Mediator(new DoThis(), new DoThat(), new DoTheOther());

Working to achieve this api, I came up with [Nimbus]. To see how it works, I incorporated nimbus into this blog.

Here is the controller that displays a blog post [before]:

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

Here is the same controller [after]:

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

I'll stipulate that the net effect is that I've simply shifted the code around. My assertion is that I have shifted it to a better place. The implementation gets to decide precisely how to fulfill the message contract and it can optimize aggressively.

Notice that the controller no longer is dependent upon `IPostVault` and `IContentStorage`. That isn't superficial. Those [interfaces are gone]. 

I remember struggling with those interfaces. What is a Post Vault anyway? Post Repository? Post Service? Post....Locker? Blech!!! That pain was a sign that it was a superfluous interface created by my desire to "inject an interface" into the controller.

I also kept trying to balance how to leak data from the classes. Should I present the posts as one big list and let the controller figure active vs future? Should I have two lists? Three? Should they be `IEnumerable` or `IReadOnlyCollections`?

How about we make them [private]. Ahhhhh. Debate over. Much better. :-]

Here's the [mediator configuration]:

    var mediator = new Mediator();

    mediator.Subscribe<PostRequest, PostGetViewModel>(() => 
        new ISubscribeFor<PostRequest>[] { 
            new FilteredPostVault(), 
            new MarkdownContentStorage(root) });

This configuration becomes a [Rosetta stone] for understanding how the application is wired together. If we want to make a change to a particular request, we know exactly what pieces are involved. Even better, if we decide to, say, change persistence technology, we have a project plan laid before us of what pieces need to be implemented with the new technology. It's already separated into pieces to dole out to the team.

I'm passing `IMediator` into the controller via autofac. _See, containers are ok_. I also got to drop some esoteric autofac configuration for which I had links to documentation. My [abstraction surface] is being streamlined. Classes will depend on the messages, `IMediator`, or some other singleton or derivative such as `IDocumentSession`.

What does zero or one dependencies get us? 

Easy testing for one. We might actually end up with some _units_ to test.

We also get the flexibility we were looking for with the "endless interfaces" approach. Right now, it so happens that view model for displaying posts is created through the cooperation of two classes. If that should need to be one class or seven classes, we can make the change without modifying the controller dependency list. If we decide to keep posts in a database or convert the text to [sphinx] or razor, the controller doesn't notice: [OCP], [SRP], [ISP], [DIP]. Can I just throw [Liskov][lsp] in?

The [source file][nimbus source], at 135 LoC, is meant for copy/paste inclusion and modification. For instance, say you want to decorate each handler instance with logging/timing/etc. Salt to taste.

Coding nimbus was an interesting journey that led to [other realizations][partial application]. I noticed how tempting it is to add features. 

What kept me in check was [ISP]. I thought [SRP] was a stronger check against sloppy design, but I kept being able to justify responsibility. Yeah, that feature is close enough. We don't need an entirely different class for just this.

It turned out the ISP kept me on the straight and narrow. After ranting about [violating isp], I could hardly force clients to take dependencies they weren't going to use. That led me to write a bunch of code to cover the signature permutations. The resulting bloat drove me to cut features that weren't truly pertinent. 

I really enjoyed this exercise and I'm much happier with the structure of my blog code.

But I think we can do better.

[questioning ioc]: /questioning-ioc-containers
[violating isp]: /violating-isp-with-constructor-injection
[violating srp]: /violating-srp-with-constructor-injection
[foo ifoo]: /foo-ifoo-is-an-anti-pattern
[Nimbus]: https://github.com/kijanawoodard/nimbus
[nimbus source]: https://github.com/kijanawoodard/nimbus/blob/b594b02a5770bf142b19f1ab468967d5f0bab694/src/mediator.cs
[partial application]: /constructor-injection-is-partial-application
[isp]: https://en.wikipedia.org/wiki/Interface_segregation_principle
[srp]: https://en.wikipedia.org/wiki/Single_responsibility_principle
[ocp]: https://en.wikipedia.org/wiki/Open/closed_principle
[dip]: https://en.wikipedia.org/wiki/Dependency_inversion_principle
[lsp]: https://en.wikipedia.org/wiki/Liskov_substitution_principle
[before]: https://github.com/kijanawoodard/Blog/blob/300ecdf6b48190849b204dbf0ad20b5c80dfd4f4/src/Blog.Web/Actions/PostGet/PostGetController.cs
[after]: https://github.com/kijanawoodard/Blog/blob/73cbeff9998dcead1d7a3da03669486216288d7b/src/Blog.Web/Actions/PostGet/PostGetController.cs#L20
[interfaces are gone]: https://github.com/kijanawoodard/Blog/commit/73cbeff9998dcead1d7a3da03669486216288d7b#diff-3131dcc0de723f47d2adfdd9e355dafdL112
[private]: https://github.com/kijanawoodard/Blog/blob/73cbeff9998dcead1d7a3da03669486216288d7b/src/Blog.Web/Infrastructure/FilteredPostVault.cs#L12
[mediator configuration]: https://github.com/kijanawoodard/Blog/blob/73cbeff9998dcead1d7a3da03669486216288d7b/src/Blog.Web/Initialization/AutofacConfig.cs#L20
[rosetta stone]: https://en.wikipedia.org/wiki/Rosetta_Stone
[abstraction surface]: https://ayende.com/blog/154081/limit-your-abstractions-you-only-get-six-to-a-dozen-in-the-entire-app
[sphinx]: https://sphinx-doc.org/rest.html#rst-primer

---
# comments begin here

- Email: "mikepennington@gmail.com"
  Message: "<p>I'm somewhat conflicted about this blog post.  I like what you're doing, and the code is very clean and concise.  And ISP is followed such that, as you mention, units of work are separate and can be tested as actual units.  Although, on the other hand, it feels a little bit like magic.  The old way, although messier, was at-a-glance readable.  I feel like there's a middle ground somewhere.</p><p>Been enjoying your blog.  :)</p>"
  Name: "Mike Pennington"
  When: "2013-10-24 18:35:53.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Wait till next time. :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-10-24 18:38:43.000"
- Email: "joey@joeyguerra.com"
  Message: "<p>I want to see how this looks when you apply similar principles in JavaScript. </p>"
  Name: "Joey Guerra"
  When: "2013-10-24 21:48:17.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Ummmm. Me too. Rob Ashton is laying it down: <a href=\"https://github.com/robashton/look-ma-no-frameworks\" rel=\"nofollow\">https://github.com/robashton/l...</a></p>"
  Name: "Kijana Woodard"
  When: "2013-10-24 21:50:24.000"