---
title: NullOr Extension Method
published: May 19, 2010
tags: 
---

I find myself writing code like this a lot:

    public static void DoSomething(Foo foo)
    {
        var thing = foo == null ? null : foo.Thing;
    }

I thought about adding an operator like ??? to go with ?? and ?, but you can’t do that in c# and it would probably be confusing to the next programmer anyway.

So how about an extension method to wrap that up:

    public static class ObjectExtensionMethods
    {
        public static TResult NullOr<T, TResult>(this T foo, Func<T, TResult> func)
        {
            if (foo == null) return default(TResult);
            return func(foo);
        }
    }

    //usage
    public static void DoSomething(Foo foo)
    {
        var value = foo.NullOr(f => f.Property);
    }

Not a lot less typing, but a bit clearer and you’re less likely to screw up.

---
# comments begin here

