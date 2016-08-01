---
title: Cool Feature of Extension Methods
published: May 16, 2010
tags: 
---

One cool and useful feature of extension methods is the fact that a null instance can call the method.

So say you write some code like this:

    using System;
 
    namespace FizzBuzz
    {
        public class ExtensionsDemo
        {
            public static void TestString()
            {
                var s = "hello";
                var ok = s.HasValue();
 
                s = null;
                ok = s.HasValue();
            }
        }
 
        public static class StringExtensionMethods
        {
            public static bool HasValue(this string value)
            {
                return !string.IsNullOrWhiteSpace(value);
            }
        }
    }

You would expect the second call to HasValue would blow up because the string is null. But the extension method is on the class not the instance so it goes through with no problem. Very handy. In fact, while typing this post I just thought of a very good use for this…coming soon.

On a side note, I think string.IsNullOrWhiteSpace is new for C# 4.0. I just found that writing the code sample for this post. Otherwise, I had to do a null check before doing a trim and then checking the length of the string.

---
# comments begin here

