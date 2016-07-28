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
[srp]: http://en.wikipedia.org/wiki/Single_responsibility_principle
[ocp]: http://en.wikipedia.org/wiki/Open/closed_principle