---
title: Violating ISP with Constructor Injection
published: October 10, 2013
tags: 
---

One [problem with IoC containers][questioning-ioc] is that they facilitate [ISP] violations through constructor injection.

> The interface-segregation principle (ISP) states that no client should be forced to depend on methods it does not use.

Let's take a look at some typical, and very terrible, code. I'm actually astonished at how many anti-patterns can be put into so few lines of code (AP/LOC?). It makes my eyes bleed.

    public interface ICustomerService
    {
        Customer GetCustomer(int id);
        void CreateCustomer(Customer customer);
    }

    public interface IRepository<T>
    {
        T GetById(int id);
        void Add(T entity);
    }

    public interface IEmailService
    {
        void SendWelcomeEmail(Customer customer);
        void SendDailyAppStatusToOperations(Customer customer);
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IEmailService _email;

        public CustomerService(
            IRepository<Customer> repository, 
            IEmailService email)
        {
            _repository = repository;
            _email = email;
        }

        public Customer GetCustomer(int id)
        {
            return _repository.GetById(id);
        }

        public void CreateCustomer(Customer customer)
        {
            _repository.Add(customer);
            _email.SendWelcomeEmail(customer);
        }
    }

About the only thing that code is missing is an `ICustomer` class. Don't laugh. Interfaces on POCOs/DTOs have been spotted in the wild. Let's not dwell on this code or how it can be changed. 

How does it violate ISP?

![Too many methods][too-many-methods]

`CustomerService` never uses `SendDailyAppStatusToOperations`, and yet it's called out as a dependency.

To be clear, this isn't _caused_ by IoC containers. We programmers tend to have a false sense of security that if we have interfaces, we're following best practices. We're [coding to the interface]! Our blind usage of layered architectures, "noun services", and endless abstractions are more to blame.

The `IEmailService` is very typical in systems I run across. Why aren't these methods on separate interfaces? My guess is that the tools (containers, R#, moq, scm, etc) are all subtly pushing us in this direction. 

Uggggh. I could create another interface, but then I'd have to go wire it up/mock it/inject it when I have this interface that already makes sense. I mean, it's all about emailing, right? And there's this other service that uses both methods. How many different interfaces am I going to have to create here?!? I'll have to add more files. Gah, I just know there will be merge conflicts on the project file! How about I just add this one method here and type alt-enter ([R#] ftw!).

Now, let's go a step farther. Let's expand the definition of ISP to the entire contract of an object, including it's constructor. 

I've seen, and written, the following test many times:

![Too many dependencies][too-many-dependencies]
![passing test][passing-test]

`Null` is passed for a "dependency" and the test still passes.

Before you dismiss this as a [straw man] argument, how did I know I could pass `null` for `IEmailService`? Isn't that secret knowledge of the internal workings of the class? If I need to change `IEmailService`, does it affect `CustomerService`?

If `IEmailService` isn't required for "getting a customer", when is it required?  Why is it a dependency for "getting a customer"? 

This is where I lay some blame on the IoC container. If we were "new-ing" up classes manually, this would obviously be silly. Someone working on "displaying customer info" feature would balk at having to construct an `IEmailService` class to pass. You can immediately see that you need a different construct for dealing with "displaying customer info" as opposed to "creating a new customer".  In anger, the programmer will probably supply `null` and commit (hey, didn't break the build!). You could argue you should have another constructor that only takes one arg, but [that's not what your container is going to use][autowiring]. If you have a constructor with one arg, then you need some guard clauses on methods that use `IEmailService` to tell the caller to use the _correct_ constructor.

The container makes this pain disappear, and that is A Bad Thing.

Imagine the business wants to "Send an email when the customer is accessed on Tuesdays". Someone goes and codes it. Wait, why are all these "Get Customer" tests failing [every Tuesday]? You mean I have to go fix all those? Yes. Yes you do.

One could argue that this was bad test writing. You should always supply a mock of all dependencies! 

Yet isn't part of TDD writing the minimum code to make the test pass? Besides, _should_ doesn't make it so.

So, while we're going through our test suite creating mocks where we didn't need them before, let's think about this a bit. 

Is the "Customer Service class" dependent on `IEmailService` or is the "Create Customer method" dependent on `IEmailService`?

I'd say the latter, but that leads us back to [8 lines of code].

As further evidence, a thought experiment: why not inject every possible dependency and then it will already be there if we ever need it? We can use better tooling to auto-mock them for easy testing. 

Pretty horrible idea, right? Why is a dependency we only need some of the time better?

Too many dependencies contribute to SRP violations as well, but I'll save that for a future post. As a preview, [Jimmy Bogard pointed out on twitter][unit testing?] that you can't really "unit test" classes written in this manner.

[questioning-ioc]: /questioning-ioc-containers
[ISP]: https://en.wikipedia.org/wiki/Interface_segregation_principle
[too-many-methods]: /content/posts/images/violating-isp/too-many-methods.png  "CustomerService doesn't use SendDailyAppStatusToOperations"
[too-many-dependencies]: /content/posts/images/violating-isp/too-many-dependencies.png  "CustomerService doesn't EmailService for this"
[passing-test]: /content/posts/images/violating-isp/passing-test.png  "the test passes anyway"
[8 lines of code]: https://www.infoq.com/presentations/8-lines-code-refactoring
[unit testing?]: https://twitter.com/jbogard/status/387977767209672704
[straw man]: https://en.wikipedia.org/wiki/Straw_man
[autowiring]: https://code.google.com/p/autofac/wiki/Autowiring
[coding to the interface]: https://stackoverflow.com/questions/4456424/what-do-programmers-mean-when-they-say-code-against-an-interface-not-an-objec
[R#]: https://stackoverflow.com/questions/95188/what-is-your-favorite-resharper-visual-studio-shortcut

---
# comments begin here

- Email: "landon.poch@gmail.com"
  Message: "<p>I wish all the programmers I'll ever have to work with would read this post and apply it.  This is a huge problem because it creates massively convoluted code.  For me, saying that this is an ISP violation is simply a more specific way of saying people are abusing scope.  If you don't need a dependency at the class level, then why is it there?</p><p>Why don't I just define everything as static and assume that everything depends on everything?  Because it's a tangled mess that's why.  The violation in your example is pretty much the same thing, just to a lesser degree.  The bottom line is, always keep variable scope as minimal as possible.  Great programmers have always been minimalists.</p><p>A couple of thoughts on IoC...</p><p>I agree that IoC isn't causing the problem here but it definitely is an accomplice.  Having the classes \"newed\" up automatically by the container makes these things easier to miss.  I've also seen performance problems occur when pulling a new instance of a class from the container because it has a bunch of needless dependencies.  You can avoid these problems though by just being careful and not abusing the power the container gives you.</p><p>I also enjoyed an article by  David Heinemeier Hansson where he explains why dependency injection is obsolete in dynamic languages.  <a href=\"https://david.heinemeierhansson.com/2012/dependency-injection-is-not-a-virtue.html\" rel=\"nofollow\">https://david.heinemeierhansson...</a>.  I agree with the idea that a pattern in one language might just be a feature that is built into the semantics of another language.</p><p>I've seen a lot of people put waaaaay too much weight behind dependency injection and ignore a lot of more important overarching principles like SRP, OCP, LSP, ISP and IoC.  Dependency injection isn't even a principle really.  It's more of an implementation detail of a principle called IoC.  I think a lot of times we focus on the wrong things and miss the really important programming concepts.  So many people are obsessed with the buzz words that they miss the underlying value.  Often programmers will use things that aren't necessary and be completely ignorant to other basic things that are arguably way more important.</p>"
  Name: "Landon Poch"
  When: "2013-10-11 04:51:41.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Excellent points. Thanks for the DHH link. One thing that egged me to write this post was stumbling across some FP threads which said things like \"and that leads to asinine things like TDD and IoC containers\".</p><p>To your point, if merely changing languages removes the need for a tool, maybe this isn't a core programming need at all but incidental complexity introduced by your language of choice.</p><p>But even then, can the problem be reshaped so that the tool isn't needed. If it hurts when you do that, don't do that. Don't make a tool to hurt yourself more efficiently.</p>"
  Name: "Kijana Woodard"
  When: "2013-10-11 15:07:08.000"