---
title: #Winning with C# Interactive
published: July 29, 2016
tags: blog, csharp
---

For my blog engine, I had a static C# class that contained a list of metadata about each post. That means when adding a new post, I had to add an element to the Posts list and create the markdown file.

Instead, I've decided to keep the post metadata in the file itself. For the format, I followed the [Jekyll Front Matter] convention.

I was facing the tedious task of typing all the metadata in the posts when I remembered that VS 2015 comes with [C# Interactive]. 

It was stunningly simple. I put the list of posts into the interactive window. Then iterated the list, formatted the front matter and prepended it to the post files. No assemblies, no compiling, no debugging. Done.

Full disclosure: I didn't write it perfectly the first time, but I just discarded the changes in git and ran the interactive script until I got it right.

I can already see an improvement writing this post. Just create the post.mardown file and go.

Hmmmm. Now I'm wondering if I should add the yaml comments at the bottom of the post file. I had thought I would use a library to parse the metadata, but none were quite what I wanted and it turned out to be [not that much code]. Since I'm doing my own parsing up front, I can snip out the comments section before passing the post text to the markdown processor. Hmmmm.

[Jekyll Front Matter]: https://jekyllrb.com/docs/frontmatter/
[C# Interactive]: https://www.hanselman.com/blog/InteractiveCodingWithCAndFREPLsScriptCSOrTheVisualStudioInteractiveWindow.aspx
[not that much code]: https://github.com/kijanawoodard/Blog/blob/7315ca32cfe0334c7c75f0913e88bba9a6cfdeed/src/Blog.Web/Infrastructure/MarkdownSharpContentStorage.cs#L63

---
# comments begin here

