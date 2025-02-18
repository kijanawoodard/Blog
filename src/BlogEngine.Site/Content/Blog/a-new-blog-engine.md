---
title: A New Blog Engine
published: 2025-02-17
lead: A look back at the original blog engine and the transition to a new static site built with BlazorStatic highlighting the changes.
tags: [blog, static-sites, blazor]
---

Welp. It's been a while. I finally got around to updating my blog engine.

As recap, the original blog engine was a custom ASP.NET MVC application that I built in 2010. It used markdown for the content and ran on azure web apps.

I've wanted to move to a static site for a while, but never found the time.

I recently used [BlazorStatic][blazorstatic] for a [bowling project][bowling] and it was pretty seamless for building a static site with blazor.

Right now, the styles are copied from the old site. I was able to port over the blog posts relatively easily. 

I got rid of the contact form. That only resulted in spam.
I got rid of the archive page. It ended up being a copy of the home page.

RSS is not currently working. I'm not sure if anyone cares.

I need to get rid of Google+. LoL.

Oh the navigation between posts is gone.

Blazor Static did give me tags "for free". That's nice.

I used cursor to generate the leads for the posts. When I checked the diffs, they looked a bit repetitive. I'm not sure if I'll keep them.

[blazorstatic]: https://blazorstatic.net/
[bowling]: https://www.5obowling.com/
