Sorry to frighten you in this [Hallow's eve][hallow], but this post is _not_ about [yet][shortbus] [another][nimbus] [mediator][liaison post]. :-]

After finishing [Liaison], I found myself [coding in anger]. What else could I cull from my stack? The obvious answer:

Kill the IoC Container.

Inspired by Ayende's [IoC container in 15 lines of code], I wrote [Vessel][vessel source] as a stripped down IoC on which to hang the mediator and any other singletons.  I bloated it up with some extra features, like registering a constructor function, but it's still small enough to read without scrolling. Vessel doesn't even have it's own GitHub repo yet.

For so few lines, I was able to remove both Autofac packages from my project. Right away, I'll stipulate that my container usage on this blog is beyond trivial. If I find myself in trouble, I can install-package my way back home again.

I think I am seeking bedrock. I want to _know_ the pain that causes me to use a framework or library. I am _way_ up the abstraction hierarchy here, but digging.

One [interesting discovery][vessel controller registration] I made through this exercise: a controller is a class that has exactly one dependency and that dependency is IMediator. I think this fact can be exploited in the future, but for the moment, I'm happy with this small victory.

[hallow]: http://en.wikipedia.org/wiki/Halloween
[shortbus]: https://github.com/mhinze/ShortBus
[nimbus]: /introducing-nimbus
[liaison post]: /introducing-liaison
[Liaison]: https://github.com/kijanawoodard/Liaison
[coding in anger]: http://programmers.stackexchange.com/a/98103
[IoC container in 15 lines of code]: http://ayende.com/blog/2886/building-an-ioc-container-in-15-lines-of-code
[vessel source]: https://github.com/kijanawoodard/Blog/blob/b67089168f218140eb3a06da1571ed94b593e377/src/Blog.Web/Infrastructure/Vessel.cs#L20
[vessel controller registration]: https://github.com/kijanawoodard/Blog/blob/b67089168f218140eb3a06da1571ed94b593e377/src/Blog.Web/Initialization/VesselConfig.cs#L29