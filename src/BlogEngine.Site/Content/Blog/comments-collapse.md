---
title: Comments Collapse
published: 8/1/2016 11:18:51 AM
tags: blog, csharp
---

At the end of my [previous post], I considered merging the comments into post file. 

I couldn't help myself. ;-)

With a little C# interactive, I now have half the files. Here's the code I typed into the interactive window:

    var files = Directory.EnumerateFiles(@"content/posts/", "*.markdown");

    foreach (var file in files)
    {
        var post = File.ReadAllText(file).Trim();
        var comments = File.ReadAllText(file.Replace(".markdown", ".comments.yaml")).Trim();
        var output = $"{post}{Environment.NewLine}{Environment.NewLine}---{Environment.NewLine}# comments begin here{Environment.NewLine}{Environment.NewLine}{comments}";
        File.WriteAllText(file, output);
    }

The instructions for comments are changed to reflect that there's only one file now. 

There's a [nice wiki on c# interactive]. One thing to note, ReSharper maps the History Navigation keys [`Alt+UpArrow`] to something else. I decided to map it back, but scope the assignment to the interactive window.

[previous post]: /winning-with-c-sharp-interactive
[nice wiki on c# interactive]: https://github.com/dotnet/roslyn/wiki/Interactive-Window

---
# comments begin here

---