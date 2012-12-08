I mentioned that I got an idea while writing the post on [extension methods]. I realized that you can null check using this technique.

It get’s annoying writing this code over and over:

<script src="https://gist.github.com/4237737.js?file=nullcheck-problem.cs"></script>

I had considered using a [`NotNull<T>` object][null object] to take care of null checking.

But with extension methods like this:

<script src="https://gist.github.com/4237737.js?file=nullcheck-solution.cs"></script>

Much nicer. The overloads can facilitate whatever messaging level you desire.

This is all probably a moot point with [Code Contracts in .net 4.0][code contracts]. To get Code Contracts working in VS2010, you have to [download the code from DevLabs][DevLabs]. That caught me off guard because the code contracts namespace is available by default in VS2010, but the actual code analysis was not.

Still, in the right situations I’d like to work on avoiding null altogether with the [Null Object Pattern] or [immutable classes].

[extension methods]:http://kijanawoodard.com/cool-feature-of-extension-methods
[null object]: http://journal.stuffwithstuff.com/2008/04/08/whats-the-opposite-of-nullable/
[code contracts]:http://mariusbancila.ro/blog/2009/05/31/code-contracts-in-visual-studio-2010/
[DevLabs]:http://msdn.microsoft.com/en-us/devlabs/dd491992.aspx
[Null Object Pattern]:http://en.wikipedia.org/wiki/Null_Object_pattern
[immutable classes]:http://weblogs.asp.net/bleroy/archive/2008/01/16/immutability-in-c.aspx