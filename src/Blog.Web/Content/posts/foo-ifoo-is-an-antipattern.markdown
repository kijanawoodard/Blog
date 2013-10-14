This simple code, used in so many examples, has always bothered me.

	class Foo : IFoo

The idea is that we have abstracted an interface so we can be [solid] and our [IoC container][questioning] will even [make this easier][ninject]. We must be on the right path. Right?

Wrong.

One goal of dependency inversion is that we can swap out implementations. Take this example.

	class Authentication : IAuthentication

We're basically declaring that we haven't thought about this very much and we're just typing away, brain off. What would another implementation even be called? `Authentication2`?

Instead we can do this:

	class Oauth2Authentication : IAuthentication

Immediately, we get the idea that other implementations might be:
	
	class ActiveDirectoryAuthentication : IAuthentication
	class LdapAuthentication : IAuthentication
	class SamlAuthentication: IAuthentication

`Foo: IFoo` is a give up.


[solid]: http://en.wikipedia.org/wiki/Dependency_inversion_principle
[questioning]: /questioning-ioc-containers
[ninject]: https://github.com/ninject/ninject.extensions.conventions