Well [nimbus][introducing nimbus], you had a great run, but now it's over. Make room for [Liaison].

While I was building nimbus, something was nagging me. It was great and flexible and [web scale] and all, but...

[Nimbus] is utter bloatware!

[Mike Pennington] summed it up in the comments:

> I'm somewhat conflicted about this blog post. I like what you're doing, and the code is very clean and concise. And ISP is followed such that, as you mention, units of work are separate and can be tested as actual units. Although, on the other hand, it feels a little bit like magic. 

Even with so much magic removed, so much magic remained. There are so may subtle "features". You can mix void handlers with result handlers. You can use base types for message handlers to generalize them. You can choose scalar vs class results. There's a lot squeezed in there. 

You can also run into trouble. If you use handlers with a mix of result types or `Send` using a type not used in `Subscribe`, you get a RuntimeBinderException. I [added comments] to give you a head's up, but I found myself lost a couple of times.

I considered making a "strict mode" for nimbus that would force throw if you didn't use the same types for handlers and subscribe. You would have to decorate any non-conforming handlers. 

Then I started to think: what work is nimbus really doing? 

Nimbus is passing the message and result to each handler in the order specified. Ok, what if we just code that? 

The goal is to isolate the _units_ from eachother and separate the organization of the units from the units themselves.

Now the mediator configuration for Posts on this blog looks like this:

	mediator.Subscribe<PostRequest, PostGetViewModel>(message =>
	{
		var result = new PostGetViewModel();
		result = new FilteredPostVault().Handle(message, result);
		result = new MarkdownContentStorage(root).Handle(message, result);
		return result;
	});

It could be easily cut down to two lines, but I found this more readable. It should be clear now how the message becomes a result.



The [Liaison source code] is now 60 lines. About half of that is cruft derived from the fact that c# has void Actions and Funcs as opposed to having Func<Unit>. I thought about forcing a result to reduce the LoC, but I'd rather have a nicer api.

Another nice side effect of the simpler code is a [3x performance boost] vs nimbus. I'm happy with 9M operations per second.

[introducing nimbus]: /introducing-nimbus
[nimbus]: https://github.com/kijanawoodard/nimbus/blob/507a3a9ba81e3af640d877158b8168f1e74e27f3/src/mediator.cs
[Liaison]: https://github.com/kijanawoodard/Liaison
[web scale]: http://mongodb-is-web-scale.com/	
[Mike Pennington]: www.linkedin.com/in/mikepennington
[added comments]: https://github.com/kijanawoodard/nimbus/blob/507a3a9ba81e3af640d877158b8168f1e74e27f3/src/mediator.cs#L88
[liaison source code]: https://github.com/kijanawoodard/Liaison/blob/e0d6aa9be055a1da227aa5d0782bdaae204a5221/src/Liaison/Mediator.cs
[3x performance boost]: https://github.com/kijanawoodard/Liaison/blob/master/src/Liaison.Tests/Performance.cs#L87