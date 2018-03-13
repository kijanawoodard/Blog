---
title: A Tale of Scope Creep
published: November 05, 2013
tags: 
---

[Zach Burke] mentioned to me that he wanted to add a 404 page to his new blog. Sounds like a good idea, lets do it.

I assumed I was going to [configure httpErrors][httperrors], but I figured I'd google a bit anyway. Turns out, there is quite a [debate][404 debate] about 404 pages with asp.net mvc. I decided I didn't really care about the nuances and I wanted to get the feature done. Good enough. [Commit][404 commit].

Next I decided that the 404 page should display the post archive so that the user can choose a post that exists. Hmmm. Ok, a little _scope creep_: how about we have [an independent archive page][archive]. Fine. Using the [mediator], it was straight forward to implement. [Commmit][archive commit].

So far so good. But the archive is rendered with the full layout on the 404 page. We don't want duplicate headers and sidebars. The easy answer is to write some code like this in the controller.

    if (ControllerContext.IsChildAction)
        return PartialView(model);
    else
        return View(model);

Yuck. I don't like that. How can we remove this logic from our controller? I need something like [FubuMVC Behaviors], but we don't have those in asp.net mvc.

After a quite a bit of [stumbling around], using an `IActionInvoker` derived from the built-in `ControllerActionInvoker` seemed to fit the bill pretty well. I'm not happy with the implementation of the class at all. It is a hack and it shows, but we're here to ship features, not build ivory towers. [Commit][partial commit].

I used [Vessel] to [wire] the `IActionInvoker` to the controller. In some sense, using Property Injection this way violates our sensibilities. My view is practical: I don't really want to do it this way, but this is what asp.net mvc gives me. I _don't_ want the controller to have to muck about with setting their own ActionInvoker and I _really_ don't want a controller base class. Yet, unless I'm ready to switch to [FubuMVC] or [OpenRasta], I'm not going to get a nice pipeline to work with. Using Property Injection seems like a reasonable compromise. 

I've also decided to continue to leave the "pain" of manual controller setup in place. Connecting the action invoker was dead simple since there were no [container incantations] to consider. I also find that it makes me _think_ about things like request pipelines, behavior chains, and the true responsibilities of a controller. 

[404 page] done.

Side notes.

It turns out that setting the layout in the view overrides the partial view behavior, so I had to [remove the layout declaration][remove layout]. That led to adding a ViewStart page. _Scope creep_. 

Conceptually, I like the views hierarchy to be composed by "something outside of themselves". If the layout is hard coded in the view, it's hard to reuse in another layout. I _think_ I would like that to be more specific than a ViewStart file, but I don't have bearings on an alternate solution.

It also turns out that the `Application_EndRequest` wasn't getting called when running on Azure Websites, aka _production_. Found an [SO post] that solved the problem. [Commit][passthrough]. This scenario highlights the value of pushing to production often. The bug simply doesn't happen in dev/test. It _only_ happens in production. Because the prod deploy was so small, it was easy to grok the issue and fix it.

The more I use [git], the more I like tiny commits that address a single issue. I don't bother to create tickets for personal projects, but in my head, I try and reason about the simplest way to solve a problem and then only commit code for that problem. Other issues I see along the way, I will either code them, but [commit][copyright commit] them individually, or make a note and come back to them later. Getting to _done_ is vital.

[Zach Burke]: https://www.throw-up.com/building-openresty
[httperrors]: https://stackoverflow.com/questions/3554844/asp-net-mvc-404-handling-and-iis7-httperrors
[404 debate]: https://stackoverflow.com/a/9026907/214073
[404 commit]: https://github.com/kijanawoodard/Blog/commit/48ed632e7045522e1404f1739dcc2cd982a63697
[archive]: /archive
[mediator]: /introducing-liaison
[archive commit]: https://github.com/kijanawoodard/Blog/commit/ce65020fdb81e06dab3a70365c7588407e695f1e
[FubuMVC Behaviors]: https://lostechies.com/chadmyers/2011/06/23/cool-stuff-in-fubumvc-no-1-behaviors/
[stumbling around]: https://github.com/kijanawoodard/Blog/blob/728c10ec6608cac03644454a7a38b7376bd10d71/src/Blog.Web/Infrastructure/PartialViewActionInvoker.cs#L8
[partial commit]: https://github.com/kijanawoodard/Blog/commit/728c10ec6608cac03644454a7a38b7376bd10d71
[vessel]: /introducing-vessel
[wire]: https://github.com/kijanawoodard/Blog/commit/97dee8e93dd305436e7687892bebbcdfeba0b9de
[FubuMVC]: https://fubumvc.github.io/
[openrasta]: https://openrasta.org/
[container incantations]: https://docs.structuremap.net/ConstructorAndSetterInjection.htm
[404 page]: /oops
[remove layout]: https://github.com/kijanawoodard/Blog/commit/728c10ec6608cac03644454a7a38b7376bd10d71#diff-8bc27b9c14ab2cf27debd6ecd280be8eL5
[SO post]: https://stackoverflow.com/a/18938991/214073
[passthrough]: https://github.com/kijanawoodard/Blog/commit/c5bcffeef6bc3e10c8fcf635ca5a8bff26d69357
[git]: https://git-scm.com/
[copyright commit]: https://github.com/kijanawoodard/Blog/commit/8f3ae65d841bab2bc6287f41923a269a458adf94

---
# comments begin here

