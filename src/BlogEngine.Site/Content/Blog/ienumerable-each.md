---
title: IEnumerable Each()
published: May 09, 2010
tags: 
---

I’ve wanted to write an each statement for IEnumerable for a while, but haven’t bothered, mostly because other devs had decided to translate everything to List anyway so I just used .ForEach or a straight foreach as appropriate. A comment on my [Avoiding FizzBuzz] post by Matt Taylor spurred me to do an implementation.

Anyway, an Each() extension method on IEnumerable is trivial:

    public static class IEnumerableExtensionMethods
    {
        public static void Each<T>(
            this IEnumerable<T> list
          , Action<T> action)
        {
            foreach (var item in list)
                action(item);
        }
    }

[Avoiding FizzBuzz]:https://kijanawoodard.com/avoiding-fizzbuzz

---
# comments begin here

- Email: "landon.poch@gmail.com"
  Message: "<p>You probably already know this but just for fun: <a href=\"https://blogs.msdn.com/b/ericlippert/archive/2009/05/18/foreach-vs-foreach.aspx\" rel=\"nofollow\">https://blogs.msdn.com/b/ericli...</a></p><p>When using expressions they should be for evaluation, not for side effects.  I found myself using SomeList.ForEach(x =&gt; { some action }) for a while.  I stopped using it after reading that just as a way to build good functional programming habits.  Even though { some action } isn't really an expression because it doesn't return anything, it just seems a little strange.</p>"
  Name: "Landon Poch"
  When: "2013-03-17 05:31:13.000"