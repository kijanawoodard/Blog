---
title: Vessel Modules
published: November 01, 2013
tags: 
---

After implementing [Vessel], I was curious what it would be like to add a module system. To do this, I added a [RegisterModules] method that scans for classes implementing `IModule` and executes them.

Doing this allows us to [define our mediator functionality in context][use module]. I like this because it allows us to add new features without having to modify a central registry.

The down side is we lose some Application level [legibility]. We are exchanging that for Feature level legibility. Vessel allows us to arrange your projects as we see fit. 

The [central configuration] is now fairly minimal with a small amount of duplication to satisfy [ISP].

I think Vessel is now complete, perhaps a bit bloated by features. Maybe some day I'll add a way to specify assemblies to scan or a plugin folder if the need arises.

I considered a special hook to register controllers given that we found that Controllers always take exactly one dependency. However, I'd like to live with that "[pain]" for the moment and see if it can inspire better solutions.

[Vessel]: /introducing-vessel
[RegisterModules]: https://github.com/kijanawoodard/Blog/blob/45887586ac446a628292fe1cd7b11673b9cc017d/src/Blog.Web/Infrastructure/Vessel.cs#L48
[use module]: https://github.com/kijanawoodard/Blog/blob/45887586ac446a628292fe1cd7b11673b9cc017d/src/Blog.Web/Actions/PostGet/PostGetController.cs#L16
[legibility]: https://www.ribbonfarm.com/2010/07/26/a-big-little-idea-called-legibility/
[central configuration]: https://github.com/kijanawoodard/Blog/blob/45887586ac446a628292fe1cd7b11673b9cc017d/src/Blog.Web/Initialization/VesselConfig.cs#L15
[isp]: https://en.wikipedia.org/wiki/Interface_segregation_principle
[pain]: https://github.com/kijanawoodard/Blog/blob/45887586ac446a628292fe1cd7b11673b9cc017d/src/Blog.Web/Actions/PostGet/PostGetController.cs#L16

---
# comments begin here

