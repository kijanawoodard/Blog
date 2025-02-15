---
title: FizzBuzz++
published: May 09, 2010
tags: 
---

Thanks to Matt Taylor sending me email comments about my [Avoiding FizzBuzz] post, I decided to jazz it up a bit.

This whole blog is dedicated to avoiding FizzBuzz type questions in general, so I figure the more code posts the better.

Using my [IEnumerable Each extension method][enumerable], I tried out this version of FizzBuzz:

    Action<int> printnum = num =>
    {
        var value = num % 15 == 0 ? "FizzBuzz"
                  : num % 5 == 0 ? "Buzz"
                  : num % 3 == 0 ? "Fizz"
                  : num.ToString();
 
        Console.WriteLine(value);
    };
 
    Enumerable.Range(1, 100).Each(printnum);

Notice I’m still trying to decompose statements into rough single responsibility. I’m still going for readability/maintainability over trying to minimize statement count. At the same time, I always strive to reduce typing by staying pretty DRY. It’s a balancing act.

Thoughts?

[Avoiding FizzBuzz]:https://kijanawoodard.com/avoiding-fizzbuzz
[enumerable]:https://kijanawoodard.com/ienumerable-each

---
# comments begin here

- Email: "larry@mcnutt-consulting.com"
  Message: "<p>My biggest pet peeve is people who post code do not include the using/imports required to cut/paste/run the snippet!  I'm lazy!</p>"
  Name: "Larry"
  When: "2010-05-10 02:56:38.000"
- Email: "kijanawoodard@wyldeye.com"
  Message: "<p>Haha. I didn't post them because I didn't write them. Wrote it all in one file. I'm lazier!</p><p>Paste into static Main of your choice.</p>"
  Name: "kijana"
  When: "2010-05-10 03:14:03.000"
- Email: "luv2code+kijanassite@gmail.com"
  Message: "<p>I use <a href=\"https://www.linqpad.net/\" rel=\"nofollow\">linqpad</a><a> for these kinds of things.  It's pretty handy for pasting a code snippet and hitting \"run\".</a></p>"
  Name: "Matt T"
  When: "2010-05-10 03:21:57.000"