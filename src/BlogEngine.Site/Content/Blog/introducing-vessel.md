---
title: Introducing Vessel
published: October 31, 2013
tags: 
---

Sorry to frighten you on this [Hallow's eve][hallow], but this post is _not_ about [yet][shortbus] [another][nimbus] [mediator][liaison post]. :-]

After finishing [Liaison], I found myself [coding in anger]. What else could I cull from my stack? The obvious answer:

Kill the IoC Container.

Inspired by Ayende's [IoC container in 15 lines of code], I wrote [Vessel][vessel source] as a stripped down IoC on which to hang the mediator and any other singletons.  I bloated it up with some extra features, like registering a constructor function, but it's still small enough to read without scrolling. Vessel doesn't even have it's own GitHub repo yet.

For so few lines, I was able to remove both Autofac packages from my project. Right away, I'll stipulate that my container usage on this blog is beyond trivial. If I find myself in trouble, I can install-package my way back home again.

I think I am seeking bedrock. I want to _know_ the pain that causes me to use a framework or library. I am _way_ up the abstraction hierarchy here, but digging.

One [interesting discovery][vessel controller registration] I made through this exercise: a controller is a class that has exactly one dependency and that dependency is IMediator. I think this fact can be exploited in the future, but for the moment, I'm happy with this small victory.

[hallow]: https://en.wikipedia.org/wiki/Halloween
[shortbus]: https://github.com/mhinze/ShortBus
[nimbus]: /introducing-nimbus
[liaison post]: /introducing-liaison
[Liaison]: https://github.com/kijanawoodard/Liaison
[coding in anger]: https://programmers.stackexchange.com/a/98103
[IoC container in 15 lines of code]: https://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code
[vessel source]: https://github.com/kijanawoodard/Blog/blob/b67089168f218140eb3a06da1571ed94b593e377/src/Blog.Web/Infrastructure/Vessel.cs#L20
[vessel controller registration]: https://github.com/kijanawoodard/Blog/blob/b67089168f218140eb3a06da1571ed94b593e377/src/Blog.Web/Initialization/VesselConfig.cs#L29

---
# comments begin here

- Email: "landon.poch@gmail.com"
  Message: "<p>The one thing I'm not sure about is how you've IoC component depends on MVC.  If I wanted to use this in a console app or a WPF project I'd have to create a reference to MVC for the IDependencyResolver interface.</p>"
  Name: "Landon Poch"
  When: "2013-11-01 20:47:01.000"
- Email: "landon.poch@gmail.com"
  Message: "<p>**you're not you've :D</p><p>I guess if I want to edit my previous posts I shouldn't be posting as a guest.</p>"
  Name: "Landon Poch"
  When: "2013-11-01 20:48:27.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>It doesn't depend on MVC, only VesselDependencyResolver does: <a href=\"https://github.com/kijanawoodard/Blog/blob/0109d6256b59e23478338ca42960737bd1a98ffc/src/Blog.Web/Infrastructure/Vessel.cs#L81\" rel=\"nofollow\">https://github.com/kijanawooda...</a></p><p>And that only exists to connect Vessel to MVC. The rest you can take use wherever.</p><p>If this was in nuget, VesselDependencyResolver would be in a separate package named Vessel.Mvc. As is, don't copy paste that class. :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-11-01 21:11:15.000"
- Email: "landon.poch@gmail.com"
  Message: "<p>Makes sense.  I guess if your framework is typically constructing a top level object (like a controller or an SVC file or whatever) you have to wire in your container somehow.</p>"
  Name: "Landon Poch"
  When: "2013-11-01 21:55:39.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Yeah. That is <a href=\"https://asp.net\" rel=\"nofollow\">asp.net</a> MVC's way to wire in your container: IDependencyResolver.</p><p>They actually have quite a few hooks into the pipeline, but for most folks, you pick your IoC and use it's IDependencyResolver implementation. This is AutoFac's version I was using: <a href=\"https://github.com/kijanawoodard/Blog/blob/785daa908deaa6caa0074974b6a25085f5efd9f0/src/Blog.Web/Initialization/AutofacConfig.cs#L26\" rel=\"nofollow\">https://github.com/kijanawooda...</a></p>"
  Name: "Kijana Woodard"
  When: "2013-11-01 22:55:17.000"
- Email: "askafif@y7mail.com"
  Message: "<p>Isn't life cycle management a 'key' (read: core) feature of an IoC container? And that leads to dependency life cycle management. I look at it as a smell when an application uses an IoC container but delegates no object life cycle and dependency life cycle management to the container.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-02 03:20:24.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Vessel provides Singleton and also \"Instance per xyz\" via the Func registration. Whether the instance is \"per request\" is determined by usage. This is inline with other IoC containers that have separate nuget packages to do per request. <a href=\"https://code.google.com/p/autofac/wiki/MvcIntegration\" rel=\"nofollow\">https://code.google.com/p/auto...</a>. That makes sense because \"per request\" means something different in a web app vs a standard console app vs a wpf app.</p><p>Using Vessel with Liaison, \"requests\" are always funneled to one call within the mediator, so resolving once and using several times in the mediation works as \"per request\".</p>"
  Name: "Kijana Woodard"
  When: "2013-12-02 16:06:16.000"