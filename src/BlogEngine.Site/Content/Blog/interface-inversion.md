---
title: Interface Inversion
published: October 15, 2013
tags: 
---

On my [previous post], [Joey Guerra] asked a question in the comments:

> Can we go further? Can't we just say that class Oauth2Authentication is the interface? I mean, why do I have to care if it implements IAuthenticate? Do I care if it implements that "interface" or do I care if it has or doesn't a method called Authenticate?

In other words, isn't the interface declared in the wrong place?

Interfaces, as we typically use them in C#, mean "these are methods and properties supported by this class".

    interface IAuthentication { bool Authenticate(); }
    class Authentication : IAuthentication

That's fine and dandy, but it's only half of the equation. The problem is that we don't usually know the _use cases_ when we write the interface. We can't necessarily coordinate the interface definition across other potential implementations either.

When we get to usage, say we find that there are two classes we can use:

    class SamlAuthentication : IAuthentication 
    { bool Authenticate() {...} }

    class ClaimsAuthentication : IAuthenticateUsers 
    { bool Authenticate() {...} }

It turns out, they have the exact same method signature. But alas, they don't have the same interface. I've been in the maddening circumstance that the two interfaces were _named_ the same, but they were from different namespaces / vendors.

Here we are with two classes that could work for us, but that don't share a common interface Wouldn't it be nice to just declare this:

    interface IAuth { bool Authenticate(); }

    //usage
    public void LoginUser(IAuth auth) { ... }


Instead of relying on the declared interfaces on the signature, we'll declare what our method needs and let the compiler sort out if the class matches the signature. A sort of interface inversion.

There's no need to make up a name for this. It's called [duck typing].

Unfortunately, in C#, the way to do duck typing is to use dynamic. That's _ok_ in some circumstances, but why can't the compiler recognize the signature compatibility. In addition, it would be really nice to _easily_ marshal a class into the shape required by an interface.

If we're not willing to use dynamic and we don't control the source of our implementation classes, we're stuck using the [adapter pattern] and writing a bunch of boilerplate code.

There's an unintended consequence of this aspect of C#. Architects want to avoid these "wasteful adapters". They also want to avoid "changes to the core". To compensate, they try to imagine a variety of ways their interfaces could be used and define those use cases as part of the interface. When we don't know, we guess and we try to "plan for the future". This leads us to [write broader interfaces][violating-isp-with-constructor-injection] than we probably need. 

Isn't [Big Design Up Front][bduf] what we were trying to avoid when we decided to made our code "loosely coupled" by adding interfaces?

 
[previous post]: /foo-ifoo-is-an-anti-pattern
[Joey Guerra]: http://www.joeyguerra.com/
[violating-isp-with-constructor-injection]: /violating-isp-with-constructor-injection
[duck typing]: https://en.wikipedia.org/wiki/Duck_typing
[adapter pattern]: https://en.wikipedia.org/wiki/Adapter_pattern
[bduf]: https://en.wikipedia.org/wiki/Big_Design_Up_Front

---
# comments begin here

- Email: "graphite@joeyguerra.com"
  Message: "<p>Oh the paradox!</p>"
  Name: "Joey Guerra"
  When: "2013-10-16 01:52:43.000"