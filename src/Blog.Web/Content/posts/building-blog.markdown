###Don't Fight the Framework
MVC 4 doesn't add trailing slashes to routes. For consistency, your routes should always end the same way. On my old wordpress blog, there was a trailing slash. I added a [url rewrite rule][ruslany] to the web.config to redirect to the canonical form. 

[ruslany]: http://blogs.iis.net/ruslany/archive/2009/04/08/10-url-rewriting-tips-and-tricks.aspx "Url Rewriting tips"