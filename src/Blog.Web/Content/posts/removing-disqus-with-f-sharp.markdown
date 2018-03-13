---
title: Removing Disqus with F# 
published: July 28, 2016
tags: fsharp
---

In my previous post, I wrote about [removing Disqus] from this blog. One tricky part was dealing with the comment export data. While you certainly can get your comments out of Disqus, they don't come in a great format. You get [a lovely chunk of xml][xml] where the posts (called threads) are disconnected from the comments (which are called posts).

I needed something that would let me parse the data without too much effort. This code only needed to be run (successfully) one time. I considered using the C# xml classes or doing something dynamic, but I wasn't thrilled at the prospect.

It turns out, [FSharp.Data] has an xml type provider. It also turns out that F# type providers are amazing.

Check out this of [code]:

    type Xml = XmlProvider<"kijanawoodard-2015-03-19T23_28_52.887832-all.xml">
    let data = Xml.GetSample()

Just like that, with no class definitons and no tedious xml and string parsing, I have a fully typed model that can be used to access the data. For example:

    data.Posts.First().Author.Name

After getting things arranged into yaml format, I wrote out the comment files and was done. I did all the processing and exlporation in [F# Interactive], so I never compiled the assembly or started the debugger.

F# could grow on me.


[removing Disqus]: /goodbye-disqus
[xml]: https://github.com/kijanawoodard/DisqusParser/blob/master/kijanawoodard-2015-03-19T23_28_52.887832-all.xml
[FSharp.Data]: https://fsharp.github.io/FSharp.Data/
[code]: https://github.com/kijanawoodard/DisqusParser/blob/master/tryout.fsx#L11
[F# Interactive]: https://fsharpforfunandprofit.com/installing-and-using/

---
# comments begin here

