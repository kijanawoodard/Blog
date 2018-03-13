---
title: Avoiding Negative Branching Tests
published: May 17, 2010
tags: 
---

I try to avoid ! tests in “if” blocks if there is a clearer way to express the idea in positive manner. Thanks to Larry McNutt for turning me on to this concept.

In my post on [extension methods], I had a string extension called HasValue. What’s the use of this?

    //negative logic:
    if (!string.IsNullOrWhiteSpace(s))
        return;
 
    //becomes:
    if (s.HasValue())
        return;

I think the second form is much more readable in that you have one less “twist” to think about.

I’ve encountered if checks in code that were just tortuous:

    if (!foo.IsAlreadyUndone())
        return;

That sort of thing just makes my brain hurt and ensures that maintenance will introduce bugs.

To get radical on my first example, I’ll sometimes go as far as removing any function calls from inside if blocks:

    //negative logic:
    if (!string.IsNullOrWhiteSpace(s))
        return;
 
    //becomes:
    if (s.HasValue())
        return;

And yes, I will use simple variables like “bool ok;” instead of “bool stringHasAValue;” because the clarity of intent is there. If this thing is ok, get out of the function. I can use this all over the code and the reader knows that nothing interesting is happening. We’ve done a check and determined we can short circuit this method. Now we can look below and determine what is interesting about this method.

[extension methods]:https://kijanawoodard.com/cool-feature-of-extension-methods

---
# comments begin here

