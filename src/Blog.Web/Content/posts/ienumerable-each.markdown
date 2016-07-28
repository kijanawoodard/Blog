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

[Avoiding FizzBuzz]:http://kijanawoodard.com/avoiding-fizzbuzz/