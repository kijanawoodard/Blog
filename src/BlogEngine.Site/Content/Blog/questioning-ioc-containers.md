---
title: Questioning IoC Containers
published: October 09, 2013
tags: 
---

Last night I awoke at 3am with a thought: maybe Greg Young has a point.

The other day, I watched Greg's [8 lines of code][eight-lines] video. I found myself agreeing, out loud, with the presentation, which is somewhat startling when you're sitting by yourself. One thing I couldn't quite swallow was his "no ioc" stance.

But then, I woke up with that _thought_.

Here's the funny bit: I didn't know _why_ my brain decided Greg had a point. So I had to back-solve the result of my own subconscious mental processing.

Now, let me state right away. I don't have any intention of convincing you of anything. I'm not convinced myself. Let's just explore a bit.

Here's a [command handler interface from ShortBus][shortbus-icommandhandler].

    public interface ICommandHandler<in TMessage>
    {
        void Handle(TMessage message);
    }

It's called from a [mediator] like this: 

    public virtual Response Send<TMessage>(TMessage message)
    {
        var allInstances = 
                _container
                    .GetAllInstances<ICommandHandler<TMessage>>();
        ...
        foreach (var handler in allInstances)
            ...
                handler.Handle(message);

I can write up my system like so:

    class DoSomething
    class DoThis : ICommandHandler<DoSomething> 
    class DoThat : ICommandHandler<DoSomething> 
    
Beautiful. This gives us a simple way to do in-memory messaging. 

To invoke all the handlers:

    mediator.Send(doSomething);
  
To add some new functionality:
    
    class DoSomething
    class DoThis : ICommandHandler<DoSomething> 
    class DoThat : ICommandHandler<DoSomething> 
    class DoTheOther : ICommandHandler<DoSomething>
    
To replace functionality:

    class DoSomething
    class DoThis : ICommandHandler<DoSomething> 
    class DoThat : ICommandHandler<DoSomething> 
    class DoAnother : ICommandHandler<DoSomething>


SRP? Check. OCP? Check. Decoupled, flexible, testable? Check. Check. Check. 

I like this.

Now, let's imagine that `DoThat` must follow `DoThis`. We need ordering. Hmmm. Well, if we need ordering, we really need a chain of events. Ok, let's sketch something.

    class DoSomething
    class DoThis : ICommandHandler<DoSomething> 
    class DoThisCompleted
    class DoThat : ICommandHandler<DoThisCompleted>

Not too bad. 

What if each handler needs to occur in a certain order? 

I guess we just create all the intermediate events. Hmmm. If each class is working from the same data and we're using an [identity map], this seems like wasted effort. But we get the benefits of messaging, so we can live with it.

Ok. Above we replaced `DoTheOther` with `DoAnother`. What if we want to write and test `DoAnother`, but we don't want it running in our production system quite yet? 

Hmmmmmmm. Well, if our intention is to eventually replace `DoTheOther`, we can make a feature branch in our [source control][git], delete `DoTheOther`, write `DoAnother`, and then deploy that feature branch to our testing environments.

I guess another option would be some kind of marker interface or attribute that tells the mediator to skip the handler. But what if someone forgets to add the marker? 

Well, we'd better do some kind of assertion on our container that it has only registered the _desired_ handlers. But at that point, we pretty much lose the benefit of our automatic wire up.

In the example code here, everything is declared together. But those handlers could be in any file. They could be in any assembly. How do we tell what handlers are going to run? 

I suppose our container might have a feature that would dump all the found instances or we could write something in the mediator that would dump that information. Then we could....inspect that manually on every build to check if..... 

This is getting complicated.

What if we want some things to happen only on certain days? 

`DoAnother` is only relevant on Wednesdays. Wait, no Thursdays. We could put that logic in `DoAnother`, but then it has too many responsibilities and needs to be altered _when_ the business changes it's mind. We could create a handler scheduler that manages that I guess.

What if the app is multi-tenant and which handlers are invoked depend on the tenant involved? What if the tenants share instances of the app? 

Hopefully, our container has an [ITenantIdentificationStrategy][autofac-multi].

Oh boy. 

So, yes. Nearly any case can be handled by carefully studying and utilizing your container of choice. And explaining all of that to someone new to the project will be....fun.

But let's back up to where we first ran into trouble. We wanted to:

1. order our handlers
2. only include the ones we need
3. have visibility into which ones will be invoked. 

Consider this:

    new Mediator(new DoThis(), new DoThat(), new DoTheOther());

Done. All the use cases are satisfied. And we have a new instance per request. If I'm honest with myself about [autofac expressions][autofac-expressions], I'm pretty much writing this code in the app boostrap routines already. 

The other thorny use cases can also be solved with pretty straight forward code.

Go check out [the 8 lines of code video][eight-lines] and check out the [EventStore repo][event-store] to see Greg's ideas in action. I found the code very discoverable.

My fear is this style involves lots of boring typing. But in exchange for a few minutes of boredom, I get a crystal clear high level overview of the entire system, all the components, and how they fit together. 

As programmers, sometimes our desire to automate overcomes our judgment and _solutions_ take more time and effort than the original problem.

I'm not quite ready to give up my container and start using `Func<Unit>` everywhere, but I sure am starting to think a lot more about whether the external tools I'm using are pulling their weight. 

And I think that was Greg Young's point.

        

[eight-lines]: https://www.infoq.com/presentations/8-lines-code-refactoring
[shortbus-icommandhandler]: https://github.com/mhinze/ShortBus/blob/master/ShortBus/ICommandHandler.cs#L5
[mediator]: https://github.com/mhinze/ShortBus/blob/master/ShortBus/Mediator.cs#L43
[identity map]: https://martinfowler.com/eaaCatalog/identityMap.html
[git]: https://git-scm.com/
[autofac-multi]: https://stackoverflow.com/a/14017242/214073
[autofac-expressions]: https://www.codeproject.com/Articles/25380/Dependency-Injection-with-Autofac#registering-a-component-created-with-an-expression
[event-store]: https://github.com/EventStore/EventStore/blob/master/src/EventStore/EventStore.Core/SingleVNode.cs

---
# comments begin here

- Email: "joey@joeyguerra.com"
  Message: "<p>Are you describing the decorator pattern here?</p>"
  Name: "Joey Guerra"
  When: "2013-10-09 16:13:01.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>I don't usually \"think in patterns\", but if you're taking about the \"solution\", then the mediator pattern...I guess.</p>"
  Name: "Kijana Woodard"
  When: "2013-10-09 16:35:37.000"