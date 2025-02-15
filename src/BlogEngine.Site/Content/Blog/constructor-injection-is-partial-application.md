---
title: Constructor Injection is Partial Application
published: October 16, 2013
tags: 
---

Looking back on my posts about [violating ISP][violating-isp-with-constructor-injection] and [duck typing][interface-inversion], a question emerges: why not declare our dependencies on the _methods_ that need them, rather than at the object level?

For example:

    void LoginUser(IAuthenication auth, string userid)

This code would be very explicit and allow us to be more granular with our dependency chain.

However, it would also be a pain in the neck. Specifically, every caller would have to take a dependency on the target's dependencies just to pass them through.

[Constructor Injection] comes along to save the day. Our dependency arrives with it's dependencies already baked in.

In functional languages, dependencies can be "baked in" via [partial application].

Constructor injection is partial application for OO.



[violating-isp-with-constructor-injection]: /violating-isp-with-constructor-injection
[interface-inversion]: /interface-inversion
[constructor injection]: https://martinfowler.com/articles/injection.html
[partial application]: https://fsharpforfunandprofit.com/posts/partial-application/

---
# comments begin here

