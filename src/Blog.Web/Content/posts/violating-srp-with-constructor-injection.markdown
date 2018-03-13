---
title: Violating SRP with Constructor Injection
published: October 11, 2013
tags: 
---

In the previous post post, we explored how constructor injection can be abused to [violate isp][violating isp]. At the end, I mentioned possible [SRP] violations as well.

Let's look at the same class:

    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private readonly IEmailService _email;

        public CustomerService(IRepository<Customer> repository, IEmailService email)
        {
            _repository = repository;
            _email = email;
        }
        ...
        public void CreateCustomer(Customer customer)
        {
            _repository.Add(customer);
            _email.SendWelcomeEmail(customer);
        }
    }

`CreateCustomer` calls the repository to add the customer and instructs the email service to send the welcome email.

Unfortunately, this is pretty standard fare.

The only way to expose the problem is to explore the boundaries of this approach as new requirements come in.

+ *When a customer is created, send them a tote bag*.  Ok, inject an IShipSwagService.
+ *Only ship tote bags if they register during the week*. Ok, add if (M-F).
+ *Don't ship bags on holidays*. Inject IHolidayService and add another if.
+ *We need statsD to show us our sign up rate*. Inject IAppStatsService
+ *They get free shipping on their first order*. Ummmm. Inject IShippingService??

This "create customer" feature starts getting complex in a hurry. Thankfully, we have R#, our trusty IoC container and a good mocking framework. We can get all these features coded up and tested in no time.

Except, SRP has silently disappeared. The `CustomerService` now has tentacles throughout the system. Nearly any change could affect `CustomerService` and changing `CustomerService` could affect the reliability of the entire application.

I suppose you noticed the [OCP] violations here as well.

As it turns out, "loose coupling", is still coupling. Our class here has to _know_ about the email, swag, and stats concepts.

So what to do instead? Messaging.

    interface ICustomerCreated
    class EmailService: IHandle<ICustomerCreated>
    class SwagService: IHandle<ICustomerCreated>
    class StatsService: IHandle<ICustomerCreated>

Now the customer service only cares about customer issues. Reasons to change: customer reasons. Clean.

Notice a secondary beneift? You don't need the `IEmailService` interface any longer because nothing depends on it. The message is the interface. Testing CustomerService just got much easier.

Decoupled is better than loosely coupled.

[violating isp]: /violating-isp-with-constructor-injection
[srp]: https://en.wikipedia.org/wiki/Single_responsibility_principle
[ocp]: https://en.wikipedia.org/wiki/Open/closed_principle

---
# comments begin here

- Email: "landon.poch@gmail.com"
  Message: "<p>Udi Dahan has a good post about domain messaging, which is very different than the enterprise messaging and integration that most people think of when they hear the term \"messaging.\"   <a href=\"http://udidahan.com/2009/06/14/domain-events-salvation/\" rel=\"nofollow\">https://www.udidahan.com/2009/0...</a>.  There are some other niceties that could be added to Udi's example but he's provided enough to make his point.</p><p>Domain messaging is lightweight and specific to the internals of the application only.  It's also noteworthy that this message bus isn't necessarily being injected into everything (static) because you often want to raise a domain event from inside an aggregate root or entity.  Injecting dependencies into those is generally a bad idea.  Handlers usually don't run on a separate thread either so you can keep control over when you need to spin off a new thread or when you don't need that added complexity.</p><p>I've also found that domain messaging helps keep your infrastructure related code decoupled from your aggregates/entites so that your domain layer is more pure.</p>"
  Name: "Landon Poch"
  When: "2013-10-11 15:34:52.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>That post was what led me to all this many years ago. I read it and immediately thought \"this is what I've been looking for, but couldn't express\".</p><p>If we keep thinking about that static bus, it gets a bit interesting and scary.</p><p>We bristle because it's a \"hidden dependency\". Udi shows how to test it [though you need to be careful on test startup/shutdown to cleanup and ensure configuration].</p><p>My counter argument is that I want messaging to \"just be there\" as part of the way things work.</p><p>Yesterday I went trolling around some sites looking for actors in Erlang, akka, etc. Then I stumbled on this: <a href=\"https://fsharpforfunandprofit.com/posts/concurrency-actor-model/\" rel=\"nofollow\">https://fsharpforfunandprofit.c...</a>.</p><p>Ummmm. I'm starting to see my problem, but I don't want to say it out loud.</p>"
  Name: "Kijana Woodard"
  When: "2013-10-11 15:45:15.000"
- Email: "feldman.sean@gmail.com"
  Message: "<p>Good post and excellent example Kijana. Thank you.</p>"
  Name: "Sean Feldman"
  When: "2013-10-26 22:20:10.000"