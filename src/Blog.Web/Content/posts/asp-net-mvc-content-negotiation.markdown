---
title: Asp.net MVC Content Negotiation
published: November 08, 2013
tags: 
---

It's long bothered me that I had a separate endpoint for Atom. After adding the [archive] endpoint, the absurdity really showed since is the same data, just in a different format.

Content negotiation to the rescue.

Web API has a decent [conneg system][web api conneg] built in. Fortunately, asp.net has enough extension points that we can craft a workable solution.

 In order to get the [404 page] working, I used a [custom action invoker][partial action invoker]. I figured that could be used as the basis for content negotiation. I leaned on several sources to pull together the [implementation][conneg action invoker].

 The code needs a bit of work, but I'm happy with the effects. You can make a request to an endpoint with an appropriate accept header or by adding an extension to the url. 
 
 The result is that the [atom feed] uses the same endpoint as the archive you can see this post in [json], [xml], [html], and [partial html][phtml]. 
 
 Hat tip to [Joey Guerra] for the extensions and phtml. He's pushed these ideas for years. I haven't always been receptive to them, but I felt there was enough value to implement them here.

One thing I learned doing this implementation is that the content negotiation should effect what action gets called rather than being merely reactive to the action result. 

 For instance, I have code to do [csv negotiation] but it isn't being used because the [current structure for posts][PostGetViewModel] isn't "flat" enough for csv. I considered some tricks using the mediator or cooking up some reflection magic to automatically flatten classes, but that seemed time consuming. Besides, what I really wanted was to get a clear opportunity in my controller to shape the output for a given mime type.

 I am halfway there as I enable atom and xml to use a [razor view][index.atom.xml] to shape the output. For csv though I'd rather have something like:

    public class MyController : Controller
    {
        ...
        public object Csv(PostRequest request)
        {
            var model = _mediator.Send<PostRequest, PostGetViewModel>(request);
            return new SomeCsvShape
            {
               Title = model.Post.Title,
               ...
            };
        }
    }    

Similarly rather than have a bunch of interfaces to support something like [HAL], an action could decorate the regular model with links, etc. 

In the end, I'd like content negotiation to have

* some default behavior that may or may not work for your type
* an outlet to shape the return result within a controller action
* an outlet to shape the result with a custom view

Interestingly, I had nearly run out of reasons to have a controller class, other than because c# code has to be in a class. Content negotiation gives new perspective to controller cohesion.

A side note on [scope creep]. I spent a fair amount of time trying to work out pdf content negotiation. After hunting around, I found [Rotativa], which looked promising, but I ran into [a bug]. It could be my issue, by while I was thinking about how to code my way out of this problem, it finally dawned on me: ctrl-p in chrome, plus [a little css][print css], pdf support done. :-]

[archive]: /archive
[web api conneg]: https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/content-negotiation
[404 page]: /oops
[partial action invoker]: https://github.com/kijanawoodard/Blog/blob/728c10ec6608cac03644454a7a38b7376bd10d71/src/Blog.Web/Infrastructure/PartialViewActionInvoker.cs
[conneg action invoker]: https://github.com/kijanawoodard/Blog/blob/0c6c3fb975deaec89035c79e9213698c7a5be5a3/src/Blog.Web/Infrastructure/ContentNegotiatingActionInvoker.cs#L14
[atom feed]: /archive.atom
[json]: /asp-net-mvc-content-negotiation.json
[xml]: /asp-net-mvc-content-negotiation.xml
[html]: /asp-net-mvc-content-negotiation.html
[phtml]: /asp-net-mvc-content-negotiation.phtml
[Joey Guerra]: https://blog.joeyguerra.com/
[csv negotiation]: https://github.com/kijanawoodard/Blog/blob/0c6c3fb975deaec89035c79e9213698c7a5be5a3/src/Blog.Web/Infrastructure/ContentNegotiatingActionInvoker.cs#L182
[PostGetViewModel]: https://github.com/kijanawoodard/Blog/blob/0c6c3fb975deaec89035c79e9213698c7a5be5a3/src/Blog.Web/Actions/PostGet/PostGetController.cs#L59
[index.atom.xml]: https://github.com/kijanawoodard/Blog/blob/0c6c3fb975deaec89035c79e9213698c7a5be5a3/src/Blog.Web/Actions/PostGet/Index.Atom.cshtml
[HAL]: https://stateless.co/hal_specification.html
[scope creep]: /a-tale-of-scope-creep
[Rotativa]: https://github.com/webgio/Rotativa
[a bug]: https://github.com/webgio/Rotativa/issues/44
[print css]: https://github.com/kijanawoodard/Blog/blob/13d109fbd53f7acc949553bded904306447cc144/src/Blog.Web/Content/css/site.css#L90

---
# comments begin here

- Email: "khalidabuhakmeh@gmail.com"
  Message: "<p>Restful Routing has the idea of a FormatResult. You can send in data, and based on the requests extension it will figure out what view you want to render it in. It works decently well, just gets goofy when dealing with ajax and routings \"keep stuff around\" default.</p><p><a href=\"https://restfulrouting.com/mappings/extras\" rel=\"nofollow\">https://restfulrouting.com/mapp...</a></p><p>OH YEAH I WENT THERE!!!! Booyah!</p>"
  Name: "khalidabuhakmeh"
  When: "2013-11-08 17:05:09.000"
- Email: "graphite@joeyguerra.com"
  Message: "<p>IIIIIIEEEEEEEEE!!!!!! IE Accepts header doesn't include text/html.</p><p><a href=\"https://www.gethifi.com/blog/browser-rest-http-accept-headers\" rel=\"nofollow\">https://www.gethifi.com/blog/br...</a></p>"
  Name: "Joey Guerra"
  When: "2013-11-11 20:14:07.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Bleh. I checked my blog and IE 10 seems ok. I don't think I care about IE 8. I'll wait for the complaints to come in.</p><p>In terms of webkit, I'm handling that by making the preference based on the order of my conneg components. It looks like I should prefer the one marked with q, but I'll leave that for another day. A day, far far far in the future.</p>"
  Name: "Kijana Woodard"
  When: "2013-11-12 03:44:21.000"