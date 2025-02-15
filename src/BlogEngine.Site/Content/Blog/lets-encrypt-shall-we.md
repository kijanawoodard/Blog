---
title: Let's Encrypt, Shall We?
created: 8/4/2016 6:47:12 PM
published: 8/5/2016 6:47:12 AM
tags: blog, notes
---

TLS Everywhere is gaining traction. I'm not convinced, but I'm not passionate enough about the issue to dig into it. I was intrigued by the [Let's Encypt] project and it's mission to provide free, pervasive, SSL Certs, and wanted to see if I could get it working [IRL].

Troy Hunt has a good post on [setting up Let's Encrypt on an Azure WebApp][troy hunt]. [Side note, Troy has a lot of great articles on his site].

The most painful part of this process was setting up the principal in Active Directory. Dealing with AD always makes my heart sink. There are so many terms and concepts that I have zero interest in learning. 

The tricky part was that the Tenant name (whatever.onmicrosoft.com) is no longer in the drop down shown in Troy's post. For me, it was in the *url* of the _old_ portal. I have no idea how to find it in the new portal, so I'm not sure what to do when the old portal [eventually?] goes away.

The next stumbling bloc I ran into was the extension was unable to access the files in the `.well-known` folder. Pro Tip: If google makes it seem like no one else has your problem, you haven't found a new problem, you've done something silly.

In my case, I had introduced a "lower case all the urls" IIS rewrite rule long ago. I won't bore you with how I figured this out, but it also turned out that it was incorrectly translating the url ... in some cases ... such that only the first letter of the filename came through. 

I added a condition to the rewrite rule to ignore files in the `.well-known` folder.

All is well......except browsers take that 301 *Permanent* rather seriously. After clearing the browser cache, I was able to see that the new rewrite rule worked as expected.

Finally, the Let's Encrypt extension was able to complete it's setup and I was able to surf to my site under https!

Yay! All done?

Not quite. The files from my recently added [azure cdn] weren't coming through. It turns out that [using TLS with custom domains is not yet supported][CDN TLS]. :-(

Oh well. I could buy a cert for this, or just wait until Q4 and see what happens. Since my domain is cookieless, has no login, and is public information, TLS can wait. Besides, I'll get to see if the Let's Encrypt extension updates the cert as advertised.

[Let's Encypt]: https://letsencrypt.org/getting-started/
[IRL]: https://www.urbandictionary.com/define.php?term=IRL
[troy hunt]: https://www.troyhunt.com/everything-you-need-to-know-about-loading-a-free-lets-encrypt-certificate-into-an-azure-website/
[azure cdn]: /setting-up-azure-cdn
[CDN TLS]: https://feedback.azure.com/forums/169397-cdn/suggestions/1332683-allow-https-for-custom-cdn-domain-names

---
# comments being here

---