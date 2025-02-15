---
title: Quickly Creating Using Namespace Directives
published: May 14, 2010
tags: 
---

If you’re not watching [TekPub videos], you’re doing yourself a disservice.

I was watching an awesome video from the [Mastering ASP.NET MVC 2 series][MVC 2], and i noticed the presenter doing something interesting.

I haven’t been too big on visual studio shortcut keys, so when I saw this it just made me cringe. If you type a class name that you don’t have a using statement for, but Visual Studio knows about the class, it will suggest using directives to you and allow you to easily add them to the using directive section at the top of the class file.

Say you want to copy a file and you type the word "File" and realize you don’t have a reference.

Now press Ctrl-. (period). You get a pop-up like this:

<figure>
    <img src="/content/posts/images/use-ctrl-period-to-insert-using.png" alt="use ctrl period to insert using" /> 
    <figcaption>Quickly insert using directives with ctrl period</figcaption>
</figure>

Now cringe like I did and think about all the time you wasted scrolling up to the top of the file and typing in the using manually. Oh but wait, you already know that File is in System.IO. What about all the times that you didn’t know what namespace the class was in and you had to go to MSDN or Google to figure that out.

I always thought people I saw doing this were using [ReSharper] and I didn’t want to "get addicted" to a third party tool I might not be able to use at the office.

Breathe deep and move on.

[TekPub videos]:http://www.tekpub.com/
[MVC 2]: http://www.tekpub.com/production/aspmvc
[ReSharper]:https://www.jetbrains.com/resharper/

---
# comments begin here

