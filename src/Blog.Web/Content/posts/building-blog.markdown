
Why not? I took a day and wrote a blog engine. I had a few goals in mind.

###Minimalism
I didn't go bare minimum, but I feel like I got pretty close. 

* No javascript. 
* No DB.
* A minimal css framework called [kube].
* CSS icons from [Font Awesome].
* [Markdown] for posts [(like this one)][this post].

I cheated a bit. I didn't want a db, but I figured a blog has to have comments, so I went with [Disqus]. So, technically, [Disqus] has a db on my behalf and loads through js. But those things aren't in my code base which means I don't have to maintain them. Win.

I'm still deciding whether I should go with [Gists for code][my gists] or [plain html code blocks][code]. Gists look a bit nicer, can be forked and downloaded, but they introduce more javascript and the code is not in the actual markdown. If the gist goes away, that code is gone. What do you think?

For CSS, I could have gone with [Twitter Bootstrap] or [Foundation], but when I was making my decision, they seemed pretty heavy weight for a blog. [Kube] plus [Font Awesome] seem to be doing quite well.

I was going for speed as well. I wanted page onload to be under 250ms. I was stoked when it was clocking in around 50ms.....until I added [Disqus] and [Gist]. The pops me to ~400ms, but I'll live with that for the features of Disqus. That fact may kill gists for me though. ;-) 

I've wondered what it would be like to write in [Markdown]. I have to say, having written these posts once in html having fought with the editor, writing in Markdown is very nice. It flows quite naturally. I like the [use of labels for links][markdown links]. It makes it easy to refer to the same link many times in a document and you cna have a nice bibliography. Check out [the raw source of this post][this post raw].

###Don't Fight the Framework
MVC 4 doesn't add trailing slashes to routes. For consistency, your routes should always end the same way. On my old wordpress blog, there was a trailing slash. I added a [url rewrite rule][ruslany] to the [web.config][urlrewrite] to redirect to the canonical form.  I also normalized to lower case and to a non-www host name.

###Fight the Framework
I wanted to see what it is like to put the model,view, and controller together in a folder instead of spreading them out across the project structure. I think I am striving for organization by feature, but this blog has too features to know if that's working. :-D

In order to get this structure to work, I had to [tweak the view engine][viewengine]. It's shockingly straight forward. This is customized exactly to the needs of this project.

###Summary
I'm pretty happy with this all in all. The idea that I spent as much time working on the posts as on the blog engine tells me I'm on the right track. 

Tell me what you think.

[kube]: http://imperavi.com/kube/
[Font Awesome]:http://fortawesome.github.com/Font-Awesome/
[Markdown]: http://daringfireball.net/projects/markdown/
[this post]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Content/posts/building-blog.markdown
[this post raw]:https://raw.github.com/kijanawoodard/Blog/master/src/Blog.Web/Content/posts/building-blog.markdown
[my gists]: http://kijanawoodard.com/fubumvc-validation-and-re-hydrating-the-view
[code]: http://kijanawoodard.com/avoiding-fizzbuzz
[Twitter Bootstrap]:http://twitter.github.com/bootstrap/
[Foundation]:http://foundation.zurb.com/

[disqus]:http://disqus.com/
[gist]:https://gist.github.com/

[markdown links]:http://daringfireball.net/projects/markdown/syntax#link

[ruslany]: http://blogs.iis.net/ruslany/archive/2009/04/08/10-url-rewriting-tips-and-tricks.aspx "Url Rewriting tips"
[urlrewrite]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Web.config#L32 "url rewrite rules"
[viewengine]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Infrastructure/AlternateViewEngine.cs "Alternate view engine"