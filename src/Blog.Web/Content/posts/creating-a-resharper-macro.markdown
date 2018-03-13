---
title: Creating a ReSharper Macro
published: October 17, 2013
tags: 
---

I wanted to get [R#] to do a little typing for me so that I can more easily add new blog posts.

A new post looks like this:
    
    new Post
    {
        Title = "Creating a ReSharper Macro",
        Slug = "creating-a-resharper-macro",
        FileName = "creating-a-resharper-macro.markdown",
        PublishedAtCst = DateTime.Parse("October 17, 2013"),
    },

The slug and filename are independently adjustable for flexibility, but they usually start out as a derivative of whatever I'm going to name the post.

I created a [R# Live Template] that looks like this:

    new Post
    {
        Title = "$title$",
        Slug = "$slug$",
        FileName = "$slug$.markdown",
        PublishedAtCst = System.DateTime.Parse("$date$"),
    },

This is great. When I activate the template, I get a chance to type for each `$variable$`. So I can type `$title$`, then `$slug$`, which gets used on two lines, and finally `$date$`.

 For `$date$`, I hooked it to a [R# Macro] that formats today's date.

 For `$slug$`, I want the title to be lowercased and the spaces to be replaced with hyphens. Unfortunately, there wasn't a built in macro that did this.

 I found a post about [extending macros] that shows how to create your own macro. The post a bit confusing and out of date for R# 7+.

 Fortunately, I found a [couple][example 1] [examples][example 2] which gave me some much needed hints to write and install the macro.

 In addition to what's outlined in the [blog post][extending macros], here are some key steps:

* When you create your project, add a reference to `JetBrains.ReSharper.Feature.Services.dll` from the R# program files bin directory. R# itself will then be able to pull in the rest of the dependencies from the bin folder as needed.
* You specify that you will transform another variable in the template through the [Parameters property][example 1] and then manipulate that value in EvaluateQuickResult.
* To install your plugin, put it in user app data, NOT the R# bin directory. It will be something like: C:\Users&lt;USERNAME&gt;\AppData\Local\JetBrains\ReSharper\v8.0\Plugins\&lt;YOUR_PLUGIN&gt;\
* Each plugin gets it's own folder 
* If the plugins directory doesn't exist, create it.

It would be nice if everything needed to write a plugin/macro was available via nuget. It would also be nice if there was a "import plugin" feature in the R# options so you didn't have to find the right directory.

If JetBrains wanted to get really crazy, they could create a Publish Plugin feature that allowed quick and easy social code sharing.

I put the [source code] for the macro on GitHub.

[R#]: https://www.jetbrains.com/resharper/
[R# Live Template]: https://www.jetbrains.com/resharper/features/codeTemplate.html
[R# Macro]: https://www.jetbrains.com/resharper/webhelp/Reference__Choose_Macro.html
[extending macros]: https://blogs.jetbrains.com/dotnet/2010/10/templates-galore-extending-functionality-with-macros/
[example 1]: https://github.com/markcapaldi/ReSharperMacros/blob/205aa6368765f742b809c11923d28f78e6e2cdca/ReSharperMacros/TestFixtureVariableExpansionMacro.cs#L49
[example 2]: https://github.com/joaroyen/ReSharperExtensions
[source code]: https://github.com/kijanawoodard/ResharperPlugins/blob/master/src/LowercaseHypens.cs

---
# comments begin here

- Email: "m.t.ellis@gmail.com"
  Message: "<p>Something like <a href=\"https://resharper-plugins.jetbrains.com\" rel=\"nofollow\">https://resharper-plugins.jetbr...</a>, you mean? :)</p><p>ReSharper plugins can be wrapped up as NuGet packages. You can find out more here: <a href=\"https://confluence.jetbrains.com/display/NETCOM/1.9+Packaging+%28R8%29\" rel=\"nofollow\">https://confluence.jetbrains.co...</a></p><p>and there's more documentation on macros here: <a href=\"https://confluence.jetbrains.com/display/NETCOM/4.04+Live+Template+Macros+%28R8%29\" rel=\"nofollow\">https://confluence.jetbrains.co...</a></p><p>We're working on something for NuGet, but for now, you need to download the sdk from the website.<br>Nice post!<br>Matt</p>"
  Name: "Matt Ellis"
  When: "2013-10-17 14:19:41.000"