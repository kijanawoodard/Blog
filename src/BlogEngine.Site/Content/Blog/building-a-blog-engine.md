---
title: Building My Own Blog
published: December 08, 2012
tags: 
---


Why not? I took a day and wrote a blog engine. I had a few goals in mind. I started by stealing, er, uh, learning from the blog of [Tim G Thomas][timgthomas] whose [source code is conveniently on GitHub][timgthomas source].

###Minimalism
I didn't go bare minimum, but I feel like I got pretty close. 

* No javascript. 
* No DB.
* A minimal css framework called [Kube].
* CSS icons from [Font Awesome].
* [Markdown] for posts ([like this one][this post]).

I cheated a bit. I didn't want a db, but I figured a blog has to have comments, so I went with [Disqus]. So, technically, [Disqus] has a db on my behalf and loads through js. But those things aren't in my code base which means I don't have to maintain them. Win.

I'm still deciding whether I should go with [Gists for code][my gists] or [plain html code blocks][code]. Gists look a bit nicer, can be forked and downloaded, but they introduce more javascript and the code is not in the actual markdown. If the gist goes away, that code is gone. What do you think?

For CSS, I could have gone with [Twitter Bootstrap] or [Foundation], but when I was making my decision, they seemed pretty heavy weight for a blog. [Kube] plus [Font Awesome] seem to be doing quite well.

I considered adding an archive page, like on Tim Thomas' blog. Since I only have 20 posts as of now, I'll just list the all posts. Once I have a hundred posts, I can come back and add that feature.

I added an Atom feed just to see what that is like. It's trivial. Now, do I need it?

###Speed
I was going for speed as well. I wanted page onload to be under 250ms. I was stoked when it was clocking in around 50ms.....until I added [Disqus] and [Gist]. The pops me to ~400ms, but I'll live with that for the features of Disqus. That fact may kill gists for me though. ;-) 

###Markdown
I've wondered what it would be like to write in [Markdown]. I have to say, having written these posts once in html having fought with the editor, writing in Markdown is very nice. It flows quite naturally. I like the [use of labels for links][markdown links]. It makes it easy to refer to the same link many times in a document and you can have a nice bibliography. Check out [the raw source of this post][this post raw].

###No DB!?! Where are the posts?
The posts are Markdown, so they are in the [content folder][my posts]. The [meta data for posts][post meta] are in classes. I have just enough infrastructure there to post into the future. I nixed some code about putting posts in "active status". Pure YAGNI. 

I also didn't want a formula for the Slug. I wanted to tweak the slug, title, and file name without having to think about the output of a method somewhere. I also avoided a base class since I just describe the shape of the class. I created a [ReSharper live template][r# templates] to output a new class and I fill in the details. Works well. 

I'm giving up the ability to "blog on the go". In reality, that never happened with my WordPress blog. Writing blog posts takes hours, for me anyway. Also, I could always edit directly on GitHub and push to production if I wanted.

###IoC
Yes. I actually typed IoC. I've been so negative on IoC lately, I wanted to give it a try again. I wanted to use it in a minimal way where it could provide value rather than blindly using it everywhere. 

I actually setup some [interfaces in the project][core]. *gasp*

I wanted posts to follow the [Open/Closed principle][solid] and have the ability to create a new blog post and have it be picked up by the infrastructure automatically without modifying some particular class. When [`FilteredPostValult.cs`][post vault] gets instantiated by the container, all the posts are there. That bit of magic is accomplished by [this line of code][post magic].

I decided to use [Autofac] since I hadn't tried that one before. It works fine IMO and there are nuget packages to get all the bits you need.

###Don't Fight the Framework
MVC 4 doesn't add trailing slashes to routes. For consistency, your routes should always end the same way. On my old wordpress blog, there was a trailing slash. I added a [url rewrite rule][ruslany] to the [web.config][urlrewrite] to redirect to the canonical form.  I also normalized to lower case and to a non-www host name.

###Except when you Fight the Framework
I wanted to see what it is like to put the model, view, and controller together in a folder instead of spreading them out across the project structure. I think I am striving for organization by feature, but this blog has too few features to know if that's working. :-D

In order to get this structure to work, I had to [tweak the view engine][viewengine]. It's shockingly straight forward. This is customized exactly to the needs of this project.

###Deployment
I used GitHub for source control, of course. I decided to see how easy it would be to launch a site on azure from github. I logged into Azure. Clicked on New -> Website. On the landing screen it asked me if I wanted to deploy from GitHub (amongst other choices). I picked the repo. Deployed. Wow.

###Summary
I'm pretty happy with this all in all. There's not a whole lot of code. The idea that I spent as much time working on the posts as on the blog engine tells me I'm on the right track. 

Tell me what you think.

[kube]: https://imperavi.com/kube/
[Font Awesome]: https://fontawesome.com
[Markdown]: https://daringfireball.net/projects/markdown/
[this post]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Content/posts/building-blog.markdown
[this post raw]:https://raw.github.com/kijanawoodard/Blog/master/src/Blog.Web/Content/posts/building-blog.markdown
[my gists]: https://kijanawoodard.com/fubumvc-validation-and-re-hydrating-the-view
[code]: https://kijanawoodard.com/avoiding-fizzbuzz
[Twitter Bootstrap]:https://twitter.github.com/bootstrap/
[Foundation]:https://foundation.zurb.com/

[disqus]:https://disqus.com/
[gist]:https://gist.github.com/

[markdown links]:https://daringfireball.net/projects/markdown/syntax#link

[timgthomas]: https://timgthomas.com/
[timgthomas source]:https://github.com/TimGThomas/blog

[my posts]: https://github.com/kijanawoodard/Blog/tree/master/src/Blog.Web/Content/posts
[post meta]:https://github.com/kijanawoodard/Blog/blob/56cc7ca343d4dfd89b42fdeed2ccc95afb400eeb/src/Blog.Web/Models/Posts.cs
[post magic]:https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Initialization/AutofacConfig.cs#L20
[core]: https://github.com/kijanawoodard/Blog/tree/master/src/Blog.Web/Core

[r# templates]:https://www.jetbrains.com/resharper/features/code_templates.html
[solid]:https://en.wikipedia.org/wiki/SOLID_(object-oriented_design)

[post vault]:https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Infrastructure/FilteredPostVault.cs
[Autofac]:https://code.google.com/p/autofac/

[ruslany]: https://blogs.iis.net/ruslany/archive/2009/04/08/10-url-rewriting-tips-and-tricks.aspx "Url Rewriting tips"
[urlrewrite]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Web.config#L32 "url rewrite rules"
[viewengine]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Infrastructure/AlternateViewEngine.cs "Alternate view engine"

---
# comments begin here

- Email: "landon.poch@gmail.com"
  Message: "<p>I'm a HUGE fan of minimalism in general.  Sometimes that philosophy can get me into a bit of trouble because I often try to oversimplify complicated issues by focusing on the absolute essential.  Even still, if I overlook a detail here or there I can go back and fix it when needed.  I find thinking like a minimalist helps produce targeted solutions.</p><p>Now if you could just convince people to publish their deployment plans and their training guides in markdown instead of in random word documents littered throughout SharePoint sites!  Then things would become indexed and searchable.  The world would be a better place.</p>"
  Name: "Landon"
  When: "2012-12-19 07:20:47.000"
- Email: "brian.t.odonnell@gmail.com"
  Message: "<p>First Comment!</p>"
  Name: "Brian O'Donnell"
  When: "2012-12-20 14:13:24.000"
- Email: "aisha_woodard@hotmail.com"
  Message: "<p>sweeeeet!</p>"
  Name: "Aisha Woodard"
  When: "2012-12-20 15:32:14.000"
- Email: "chiefpriestess@gmail.com"
  Message: "<p>I think you should stick to HTML 5. It is compact, easy and can be under your control no matter what the rest of the world it doing. It also probably loads faster than gist.</p>"
  Name: "Iya Omitade Ifatoosin"
  When: "2012-12-20 22:31:06.000"
- Email: "joey@joeyguerra.com"
  Message: "<p>I'm not convinced about using markdown as the data source. I still think HTML is better so that editors can include meta data about an article.</p>"
  Name: "Joey Guerra"
  When: "2012-12-23 13:46:44.000"