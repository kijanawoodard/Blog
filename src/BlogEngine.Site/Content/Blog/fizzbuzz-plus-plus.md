---
title: FizzBuzz++
published: May 09, 2010
lead: An enhanced implementation of the classic FizzBuzz programming exercise, demonstrating more elegant solutions using C# extension methods and functional programming concepts.
tags: [coding, csharp]
comments:
  - email: "larry@mcnutt-consulting.com"
    message: "<p>My biggest pet peeve is people who post code do not include the using/imports required to cut/paste/run the snippet!  I'm lazy!</p>"
    name: "Larry"
    when: "2010-05-10 02:56:38.000"
  - email: "kijanawoodard@wyldeye.com"
    message: "<p>Haha. I didn't post them because I didn't write them. Wrote it all in one file. I'm lazier!</p><p>Paste into static Main of your choice.</p>"
    name: "kijana"
    when: "2010-05-10 03:14:03.000"
  - email: "luv2code+kijanassite@gmail.com"
    message: "<p>I use <a href=\"https://www.linqpad.net/\" rel=\"nofollow\">linqpad</a><a> for these kinds of things.  It's pretty handy for pasting a code snippet and hitting \"run\".</a></p>"
    name: "Matt T"
    when: "2010-05-10 03:21:57.000"
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

Notice I'm still trying to decompose statements into rough single responsibility. I'm still going for readability/maintainability over trying to minimize statement count. At the same time, I always strive to reduce typing by staying pretty DRY. It's a balancing act.

Thoughts?

[Avoiding FizzBuzz]:https://kijanawoodard.com/avoiding-fizzbuzz
[enumerable]:https://kijanawoodard.com/ienumerable-each

