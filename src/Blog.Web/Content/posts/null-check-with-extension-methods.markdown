---
title: Null Check with Extension Methods
published: May 18, 2010
tags: 
---

I mentioned that I got an idea while writing the post on [extension methods]. I realized that you can null check using this technique.

It get’s annoying writing this code over and over:

    public static void ImportantMethod(string value)
    {
        if (value == null)
            throw new ArgumentNullException();
    }

I had considered using a [`NotNull<T>`][null object] to take care of null checking.

But with extension methods like this:

    public static class ObjectExtensionMethods
    {
        public static void NullCheck<T>(this T foo)
        {
            NullCheck(foo, string.Empty);
        }
 
        public static void NullCheck<T>(this T foo, string variableName)
        {
            if (foo == null)
                throw new ArgumentNullException(variableName);
        }
 
        public static void NullCheck<T>(this T foo, string variableName, string message)
        {
            if (foo == null)
                throw new ArgumentNullException(variableName, message);
        }
    }

    //usage
    public static void ImportantMethod(string value)
    {
        value.NullCheck();
    }

Much nicer. The overloads can facilitate whatever messaging level you desire.

This is all probably a moot point with [Code Contracts in .net 4.0][code contracts]. To get Code Contracts working in VS2010, you have to [download the code from DevLabs][DevLabs]. That caught me off guard because the code contracts namespace is available by default in VS2010, but the actual code analysis was not.

Still, in the right situations I’d like to work on avoiding null altogether with the [Null Object Pattern] or [immutable classes].

[extension methods]:https://kijanawoodard.com/cool-feature-of-extension-methods
[null object]: https://journal.stuffwithstuff.com/2008/04/08/whats-the-opposite-of-nullable/
[code contracts]:https://mariusbancila.ro/blog/2009/05/31/code-contracts-in-visual-studio-2010/
[DevLabs]:https://msdn.microsoft.com/en-us/devlabs/dd491992.aspx
[Null Object Pattern]:https://en.wikipedia.org/wiki/Null_Object_pattern
[immutable classes]:https://weblogs.asp.net/bleroy/archive/2008/01/16/immutability-in-c.aspx

---
# comments begin here

