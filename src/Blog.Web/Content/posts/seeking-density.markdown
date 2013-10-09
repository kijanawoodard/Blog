I've found myself making mental leaps about coding more quickly by cross pollinating the input data.

Take architectural abstractions. They've always grated on me. The better I got at writing code, the more I thought they were a waste of time...most of the time.

Yesterday I was reading the latest in a series of Ayende posts about [Limiting your abstractions][limiting abstractions].

Today I had time to kill during jury duty and read a Ribbon Farm post about [Dense Writing][dense writing].

Click.

What's always bothered my architectural abstractions is that tend to become brain-off copy/paste best practices that are adding more noise than value to the code base.

I like the idea that you should limit your abstractions in your code base. Oren says "you get a dozen" - tops!

The point is that you cannot abstract everything. You actually need to make fact-informed decisions and iterate to new decisions.

Simply declaring `IDataAbstraction<T>` doesn't make it so.

If you try to hide EF and NHibernate behind your abstraction you will be unable to optimize. For example, should you Eager load complex properties of an entity or not. Sometimes you should, sometimes you should not. The only code that knows when to do the right thing, is the calling code. Your abstraction prevents you from optimizing when and where necessary.

Finding yourself in this predicament, you have a few options.

1. Write your `IFetchingStrategy<T>` and map that against EF and NHibernate. You're wasting your life. You've got an app to build.
2. Put a method on your abstraction that maps precisely to your current concrete O/RM. Your abstraction is now garbage. Your implementations become littered with "throw not implemented exceptions" or worse, will simply do nothing creating very subtle bugs. You can no longer swap implementations.
3. Accept reality and use the O/RM you chose. Doing string replace on ISession to DBContext will be trivial compared to reworking the code around the subtleties of the new O/RM.

Similarly, swapping out SQL Server for [CouchDB] for [RavenDB] for [HyperGraphDB] is not going to be trivial simply because you whipped together some `IDataBase<T>`. These technologies have subtle, and not so subtle, differences that contribute to a decision about whether or not to use them in your project. You can't hide them behind an abstraction "in case you were wrong".

Either you are castrating the tool, meaning you might as well have chosen something else, or your abstraction is an illusion and you're wasting time with Empty Calorie Abstractions.

Now all that sounds awful unless you get the odd idea in your head that you can have more than one database in your system. Then all these decisions are much less important. But that's another story.

An aside, the writing on [RibbonFarm] demonstrates that I need to work on my writing. The entire site is worth reading if only for the mind-expanding properties of the dense writing.

[limiting abstractions]:http://ayende.com/blog/153889/limit-your-abstractions-analyzing-a-ddd-application
[dense writing]: http://www.ribbonfarm.com/2012/01/11/seeking-density-in-the-gonzo-theater/
[RibbonFarm]: http://www.ribbonfarm.com/
[CouchDB]: http://couchdb.apache.org/
[RavenDB]: http://ravendb.net/
[HyperGraphDB]: http://www.hypergraphdb.org/index