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
[Mike Pennington]: www.linkedin.com/in/mikepennington
[added comments]: https://github.com/kijanawoodard/nimbus/blob/507a3a9ba81e3af640d877158b8168f1e74e27f3/src/mediator.cs#L88
[mediator configuration]: https://github.com/kijanawoodard/Blog/blob/36b4b747c0a538d46ac418e0ed51f07e66bedb52/src/Blog.Web/Initialization/AutofacConfig.cs#L20
[liaison source code]: https://github.com/kijanawoodard/Liaison/blob/e0d6aa9be055a1da227aa5d0782bdaae204a5221/src/Liaison/Mediator.cs
[3x performance boost]: https://github.com/kijanawoodard/Liaison/blob/master/src/Liaison.Tests/Performance.cs#L87