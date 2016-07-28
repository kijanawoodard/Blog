---
title: Just use string Id for RavenDB
published: May 31, 2012
tags: 
---

I tried to fight the framework, but it isn't worth it. With RavenDB, the default expectation is to use string for Id properties and they will get generated to look like this: "posts/1". The slash causes a problem for MVC routing and it doesn't look all that great "posts/details/posts/1".

You can overcome the MVC issues pretty quickly just by making your Id an int property. Blam. It works and your route looks like "posts/details/1".

It turns out though that once you get into more interesting RavenDB features, notably indexes, using int for Id is a real PITA. Indexes run on the Server and the server still sees all the document as having Id as string list "posts/1". Your queries with int Id properties won't match and you'll be frustrated.

So I decided to switch back to string Id properties and then convert them to int for the routing. For Load&lt;Post&gt;(id), using the int works great. However, as soon as that id is used in a where clause, forget it; you have to figure out how to get the proper string representation again.

Here are [two choices for what to do with your id properties][ravenid]. 

I decided on the first option: changing the parts separator so my id looks like "posts-1". Now my routing works fine. The seo friendliness is an issue, but I'm working on apps not websites so it's ok. The urls end up being more "hackable". Say you end up with a route like "blogs/edit/1/1", I think it's easier to hack/read "blogs/edit/posts-1/comment-1".

Rule of life: Don't Fight the Framework. If you find yourself writing a lot of code to get around a framework/tool issue, either drop the framework/tool or drop your hack code. It's more trouble than it's worth.

[ravenid]: http://weblogs.asp.net/shijuvarghese/archive/2010/06/04/how-to-work-ravendb-id-with-asp-net-mvc-routes.aspx