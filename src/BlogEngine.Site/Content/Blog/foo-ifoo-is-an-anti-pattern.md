---
title: Foo: IFoo is an Anti-Pattern
published: October 14, 2013
tags: 
---

This simple code, used in so many examples, has always bothered me.

    class Foo : IFoo

The idea is that we have abstracted an interface so we can be [solid] and our [IoC container][questioning] will even [make this easier][ninject]. We must be on the right path. Right?

Wrong.

One goal of dependency inversion is that we can swap out implementations. Take this example.

    class Authentication : IAuthentication

We're basically declaring that we haven't thought about this very much and we're just typing away, brain off. What would another implementation even be called? 

    class Authentication2 : IAuthentication //???????????

We can do better.

    class Oauth2Authentication : IAuthentication

Immediately, we get the idea that other implementations might be:
    
    class ActiveDirectoryAuthentication : IAuthentication
    class LdapAuthentication : IAuthentication
    class SamlAuthentication: IAuthentication

`Foo: IFoo` is a give up.


[solid]: https://en.wikipedia.org/wiki/Dependency_inversion_principle
[questioning]: /questioning-ioc-containers
[ninject]: https://github.com/ninject/ninject.extensions.conventions

---
# comments begin here

- Email: "graphite@joeyguerra.com"
  Message: "<p>Can we go further? Can't we just say that class Oauth2Authentication is the interface? I mean, why do I have to care if it implements IAuthenticate? Do I care if it implements that \"interface\" or do I care if it has or doesn't a method called Authenticate?</p>"
  Name: "Joey Guerra"
  When: "2013-10-15 02:08:23.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>You've brought up two future posts. :-)</p><p>I would say the interface is the Authenticate method. I hinted at this at the end of <a href=\"https://kijanawoodard.com/violating-isp-with-constructor-injection\" rel=\"nofollow\">https://kijanawoodard.com/viola...</a></p>"
  Name: "Kijana Woodard"
  When: "2013-10-15 14:44:03.000"