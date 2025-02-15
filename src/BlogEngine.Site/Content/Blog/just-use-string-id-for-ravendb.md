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

[ravenid]: https://weblogs.asp.net/shijuvarghese/archive/2010/06/04/how-to-work-ravendb-id-with-asp-net-mvc-routes.aspx

---
# comments begin here

- Email: "max@mwsd.se"
  Message: "<p>Thanks for the post. Wish I've read this a week ago. Int works great at first but for indexes its just not working out good at all. I'm switching back to strings and use an ToIntId extension for use when creating view model objects.</p>"
  Name: "Max"
  When: "2013-01-30 04:12:25.000"
- Email: "khalid@aquabirdconsulting.com"
  Message: "<p>Since RavenDB doesn't care what the string is. I sometimes use a Guid or ShortGuid implementation for my ids. The nice part about using guids is you don't leak business knowledge out through your ids. Example below.</p><p>I signup for a service and see that I am Customer/1 or CreditCard/1 then that might scare me or let my competitors know where I am at in my conception. Also if you have a ballpark estimate of customers (Customer/2020) and the service charges $20/month per customer. Competitors might realize that my monthly income is $40,400.</p><p>These are things to keep in mind. If you ids are all used internally then I would just stick with the Id scheme that Raven gives you.</p><p>You do lose a feature by changing the seperator to -. If your id ends with a / then raven will put a number at the end for you. ex. \"customer/1/creditcard/\" would get a number.</p>"
  Name: "Khalid Abuhakmeh"
  When: "2013-01-30 09:04:25.000"
- Email: "khalid@aquabirdconsulting.com"
  Message: "<p>I'd also be careful with your plan if you have plans to Shard your data. Sharding in RavenDB places the Shard key at the beginning of your Id. if USA was a shard, your key would look like \"USA/Customer/1\". Your extension method should be implemented in a way that can take that into account.</p>"
  Name: "Khalid Abuhakmeh"
  When: "2013-01-30 09:16:25.000"