---
title: To Catch a Bot, Use a Honeypot
created: 8/9/2016 10:22:44 AM
published: 8/9/2016 10:22:44 AM
tags: blog
---

For my [contact] form, I needed some spam protection from the evil bots roaming the interwebs. I considered [recaptcha], but I want to minimize the amount of javascript on this site, plus it's fun to see what I can do on my own.

For inspiration, I remembered a post I read long ago from [Sam Saffron on blog spam][saffron]. In it, he talks about the habits of bots and how they love to fill out form fields. The trick then is to give the bots something to fill out that humans won't. Instead of making humans prove they are not bots, make bots prove they are human.

I recently used [formspree] which has a `_gotcha` field for this purpose. I added a similar field to my contact form and hid it using css, so humans won't be bothered by it. The results were great. The first contact that came in was marked as a bot!

After that initial success, I began experimenting a bit more. I currently have 3 different honeypot fields to see which ones the bots find irresistable. I think that the [first honeypot] will prove the most effective.

There are a few things to note. 

At first I had the `required` attribute on all the non-honeypot fields. I think this is a dead giveaway to the bots on what they can ignore. Also, I realized I didn't really need to require anything. If you want to send me a message without an email address to reply to, so be it.

Second, I'm hiding the honeypot fields via css which means screen readers will still present them to users. For that case, I made sure the labels on the fields are very clear, e.g. "This section is for robots. Humans can ignore." In the end, it won't matter since I'm not actually blocking bot posts. I'm simply marking them in the subject so I can quickly sort in my mail client. You only need to read a few words to identify a marketing email.

For now, I'm happy with this approach. I can always make it more robust later if need be. I'm curious as to how it would hold up as protection against comment spam.



[contact]: /contact
[recaptcha]: https://www.google.com/recaptcha/intro/index.html
[saffron]: https://samsaffron.com/archive/2011/10/04/Spam+bacon+sausage+and+blog+spam+a+JavaScript+approach
[formspree]: https://formspree.io/
[first honeypot]: https://github.com/kijanawoodard/Blog/blob/4dffaaadc4eb2a7e9d53849f662159ffddac7e52/src/Blog.Web/Actions/Contact/Index.cshtml#L20

---
# comments being here

---