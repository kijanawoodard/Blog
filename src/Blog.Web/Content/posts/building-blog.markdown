
###Don't Fight the Framework
MVC 4 doesn't add trailing slashes to routes. For consistency, your routes should always end the same way. On my old wordpress blog, there was a trailing slash. I added a [url rewrite rule][ruslany] to the [web.config][urlrewrite] to redirect to the canonical form.  I also normalized to lower case and to a non-www host name.

###Fight the Framework
I wanted to see what it is like to put the model,view, and controller together in a folder instead of spreading them out across the project structure. I think I am striving for organization by feature, but this blog has too features to know if that's working. :-D

In order to get this structure to work, I had to [tweak the view engine][viewengine]. It's shockingly straight forward. This is customized exactly to the needs of this project.

[ruslany]: http://blogs.iis.net/ruslany/archive/2009/04/08/10-url-rewriting-tips-and-tricks.aspx "Url Rewriting tips"
[urlrewrite]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Web.config#L32 "url rewrite rules"
[viewengine]: https://github.com/kijanawoodard/Blog/blob/master/src/Blog.Web/Infrastructure/AlternateViewEngine.cs "Alternate view engine"