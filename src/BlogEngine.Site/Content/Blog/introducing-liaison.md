---
title: Introducing Liaison
published: October 25, 2013
tags: 
---

Well [nimbus][introducing nimbus], you had a great run, but now it's over. Make room for [Liaison].

While I was building nimbus, something was nagging me. It was great and flexible and [web scale] and all, but...

[Nimbus] is utter bloatware!

[Mike Pennington] summed it up in the comments:

> I'm somewhat conflicted about this blog post. I like what you're doing, and the code is very clean and concise. And ISP is followed such that, as you mention, units of work are separate and can be tested as actual units. Although, on the other hand, it feels a little bit like magic. 

Even with so much magic removed, so much magic remained. There are many subtle "features". You can mix void handlers with result handlers. You can use base types for message handlers to generalize them. You can choose scalar vs class results. There's a lot squeezed in there. 

You can also run into trouble. If you use handlers with a mix of result types or `Send` using a type not used in `Subscribe`, you get a RuntimeBinderException. I [added comments] to give you a head's up, but I found myself lost a couple of times.

I considered making a "strict mode" for nimbus that would `throw` if you didn't use the same types for handlers and subscribe. You would have to decorate any non-conforming handlers. 

Then I started to think: what work is nimbus really doing? 

Nimbus is passing the message and result to each handler in the order specified. 

The goal is to isolate the _units_ from each other and separate the organization of the units from the units themselves. Ok, what if we just code that? 

Now the [mediator configuration] for Posts on this blog looks like this:

    mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
    {
        var result = new PostGetViewModel();
        result = new FilteredPostVault().Handle(message, result);
        result = new MarkdownContentStorage(root).Handle(message, result);
        return result;
    });

That code could be cut down to two lines, but I found this more readable. It should be clear now how the message is transformed into a result.

If we don't like inline functions, we can do this:
    
    public static void RegisterContainer()
    {
        ...
        mediator.Subscribe<PostRequest, PostGetViewModel>(Execute);
        ...
    }
    ...
    private static PostGetViewModel Execute(PostRequest message)
    {
        var result = new PostGetViewModel();
        result = new FilteredPostVault().Handle(message, result);
        result = new MarkdownContentStorage(root).Handle(message, result);
        return result;
    }

If we don't want a bunch of functions, we can use classes:

    mediator.Subscribe<PostRequest, PostGetViewModel>(message => 
        new HandlePostGetViewModel().Handle(message));
    ...
    public class HandlePostGetViewModel
    {
        public PostGetViewModel Handle(PostRequest message)
        {
            var result = new PostGetViewModel();
            result = new FilteredPostVault().Handle(message, result);
            result = new MarkdownContentStorage(root).Handle(message, result);
            return result;
        }
    }

Wait. Wait. Wait! Whoa! Whoa! Whoa! Whoa. Whoa. Whoa. Whoa.

Whoa.

Isn't that where we started??!?

Yes and no.

We've come full circle, but along the way, we've dropped a lot of dead weight and clarified our approach to code considerably. 

How do we keep from going off the reservation and making spaghetti in our Subscriptions?

* Handlers will have 0 or 1 dependencies.
* The 1 dependency will be the mediator _or_ a fully constructed singleton.
* Prefer a derivation of the singleton - `store.OpenSession()`.
* The singleton should generally be from another library - i.e. persistence lib.
* Handlers will have void/Unit return type _or_ the same return type specified in the Subscribe.

I think I prefer either the inline function or methods within the configuration class over classes. I'll try it out in a couple projects and see. As always, copy/paste into your own project and salt to taste.

The [Liaison source code] is now 60 lines. About half of that is cruft due to the fact that c# has `void Actions` as opposed to having `Func<Unit>`. I thought about forcing a result to reduce the LoC, but I'd rather have a nicer api.

Another nice side effect of the simpler code is a [3x performance boost] vs nimbus. I'm happy with 9M operations per second.

One thing I think I miss is the IHandle interface. Maybe I'm just being sentimental, but it does enforce rules for method names [Handle vs Execute vs ???]. Add the interfaces if it helps keep your codebase consistent.

On a minor note, I named nimbus with the project, solution, folders, etc all lowercase. It turns out, I prefer being idiomatic for the language in play. Javascript methods should be `doSomething()` and c# methods should be `DoSomething()`. Liaison is cased properly. 

;-]

[introducing nimbus]: /introducing-nimbus
[nimbus]: https://github.com/kijanawoodard/nimbus/blob/507a3a9ba81e3af640d877158b8168f1e74e27f3/src/mediator.cs
[Liaison]: https://github.com/kijanawoodard/Liaison
[web scale]: http://mongodb-is-web-scale.com/    
[Mike Pennington]: https://www.linkedin.com/in/mikepennington
[added comments]: https://github.com/kijanawoodard/nimbus/blob/507a3a9ba81e3af640d877158b8168f1e74e27f3/src/mediator.cs#L88
[mediator configuration]: https://github.com/kijanawoodard/Blog/blob/36b4b747c0a538d46ac418e0ed51f07e66bedb52/src/Blog.Web/Initialization/AutofacConfig.cs#L20
[liaison source code]: https://github.com/kijanawoodard/Liaison/blob/e0d6aa9be055a1da227aa5d0782bdaae204a5221/src/Liaison/Mediator.cs
[3x performance boost]: https://github.com/kijanawoodard/Liaison/blob/master/src/Liaison.Tests/Performance.cs#L87

---
# comments begin here

- Email: "graphite@joeyguerra.com"
  Message: "<p>This reminds me of a pub sub implementation, which of course, I LOVE!</p>"
  Name: "Joey Guerra"
  When: "2013-10-26 04:03:09.000"
- Email: "graphite@joeyguerra.com"
  Message: "<p>And I couldn't help but <a href=\"https://www.youtube.com/watch?v=b2F-DItXtZs\" rel=\"nofollow\">https://www.youtube.com/watch?v...</a> to the web scale link.</p>"
  Name: "Joey Guerra"
  When: "2013-10-26 14:41:41.000"
- Email: "khalidabuhakmeh@gmail.com"
  Message: "<p>This reminds me of what Fubu MVC is doing, where actions can be chained. I am still on the fence of whether I would use something like this or not, since the cognitive overhead might not be worth the addition of the code.</p>"
  Name: "khalidabuhakmeh"
  When: "2013-10-29 13:50:57.000"
- Email: "askafif@y7mail.com"
  Message: "<p>Have you looked at TInyMessenger (part of TinyIoC)? Its a more full fledged in memory bus with pub sub semantics, all rolled into one file.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-08 22:16:47.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Nice. Thanks for the heads up.</p><p>Liaison is aiming to find a minimum level of abstraction. It's more an exercise in understanding than anything. For instance, just responding to this comment, I've thought of a couple way to further simplify my stack. :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 21:29:55.000"
- Email: "askafif@y7mail.com"
  Message: "<p>And you're doing great. I love minimalism too, but often, in that pursuit I think I am probably going to miss out on important concerns that are there for a reason in other libraries. I am searching hard for a great in memory bus, that lets me do true event driven programming (think NSB API), where one can do a send and publish, and send is from 1 to many, handled by only one, and publish is from one and only one, handled by zero to many. The hardest I find in this pursuit is implementing Unit of work around the message handler, and still keeping it dead simple.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-09 22:12:40.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Thanks.</p><p>I started this journey looking at NSB 4.0 \"In Memory Publish\" and realized I really wanted to do Send, which it didn't have. I didn't realize how far that would take me. :-]</p><p>I started out with nimbus (<a href=\"https://kijanawoodard.com/introducing-nimbus/\" rel=\"nofollow\">https://kijanawoodard.com/intro...</a>) so that I could register handlers for messages anywhere, like nsb. What I quickly realized is that you get into cases like \"I need the handlers to run in a particular order\", \"This handler needs two parameters\", \"This handler needs zero parameters\", \"This handler uses the result of the work of the first 3 handlers (an Event)\".</p><p>Once there, I either needed to retreat to a \"full featured\" container, although that doesn't solve the ordering issue (see nsb's .First&lt;t&gt;(), etc), or I needed something else.</p><p>What I've found is that the \"orchestration code\" is fairly concise to write manually and keeps a lot of if/switch logic out of the handler code. Not to mention, I don't have to learn the incantations of the container.</p><p>I still owe you a blog post with more details and code. :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 23:00:52.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Also, it's interesting that you wrote this, because I was thinking of adding a \"publish\" feature, which really amounts to allowing multiple subscriptions for a message.</p><p>I've tried not to impose concepts like Command vs Event and I think I can continue to do that, but allow \"send to many\".</p><p>I haven't decided whether it should just be Send -&gt; \"send to whatever is registered\" or if I should do something like Send -&gt; \"send to subscriptions[0]\" and Publish -&gt; \"send to all subscriptions\".</p><p>I'll write the code and let it decide. :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 23:05:12.000"
- Email: "askafif@y7mail.com"
  Message: "<p>To be honest, at my last assignment, I did create an in memory pub sub mechanism to let the team get their head around event driven architecture without all the queues and distribution. We used that to move from procedural style to events and commands, and that paved the way for NSB to come in later. I am itching to write that again, but want to look around so I can borrow 'good' ideas, or even better just use something that fits the bill.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-09 23:10:47.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>I'm sorta pushing \"roll your own\" with this code since it's single file inclusion only.</p><p>I have used it in enough mini projects that I'm considering putting it on nuget just for my own sake. But I want to let the api settle a bit. I still think it's a bit bloated, if you can believe that.</p><p>The key to \"in memory publish\" in a web request is transactions. If we're honest, we really only get one. Beyond that you're pushing your luck or opening up a can of worms. If the request fatals, where are you? Can you restart? From which point?</p><p>So a real \"publish\" where \"this is an Event that happened in the past\" (meaning saved to disk somewhere for all time), isn't friendly in memory.</p><p>I _think_ I'd like to add a way for disconnected code to register \"background tasks\" within the same unit of work as the main request handler code. The background tasks would then carry out follow on options like \"send an email\", \"update the stats screen\", etc. I'm not sure that this use case is worth the mental disconnect of not being able to trivially see what's going to be saved when the commit occurs.</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 23:19:27.000"
- Email: "askafif@y7mail.com"
  Message: "<p>Also, the moment you have something that lets you do messaging, I feel its imperative to demonstrate the difference between command and event. Without that it feels messages are flying everywhere for no rhyme or reason. Hiding in the sheep's clothing of decoupling behind messages developers will write code that has all interesting side effects.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-09 23:20:07.000"
- Email: "askafif@y7mail.com"
  Message: "<p>That looks familiar to what Oren has done with Racoon blog. Interesting thoughts.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-09 23:22:09.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>That's where I stole the idea.</p><p>Oren's Limit your Abstractions series is pivotal to my line of thinking on this. If I can drop IFoo, IGoo, IThisService, and IThatService and boil things down to IMediator (or whatever), clarity is what emerges.</p><p>I find the constraint similar to ReST constraints. At first you find it hampers you. But soon, you see that what you were doing before was \"making stuff up\" and not focusing on the essence of what needed to be done. At the end, your code becomes a tightly coupled mess of Interfaces that were suppose to solve the coupling problem but didn't because you just wrote the same old procedural spaghetti code hidden behind an interface.</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 23:30:46.000"
- Email: "askafif@y7mail.com"
  Message: "<p>Couldn't agree more.</p>"
  Name: "Afif Mohammed"
  When: "2013-12-09 23:38:58.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>I know how you feel. Since I'm going for minimalism, I didn't want to impose that view _from_ Liaison. So if you want IEvent and ICommand and IHandle&lt;t&gt;, etc go ahead. Liaison won't _force_ you to do that, but it's easy to overlay. Plus, I didn't want to write \"unobtrusive mode\". :-]</p>"
  Name: "Kijana Woodard"
  When: "2013-12-09 23:48:16.000"