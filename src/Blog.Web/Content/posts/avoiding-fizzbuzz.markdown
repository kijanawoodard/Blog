---
title: Avoiding FizzBuzz
published: May 09, 2010
tags: 
---

Getting hired for a programming job means interviewing. This process is utterly necessary, but often tedious.

Interviewers have to be able to weed out the “no hopers”. The problem is, decent candidates are put off being asked “the basics”.

For me, the problem is time. We only have 45 minutes to an hour for the interview. I’d rather spend the time talking about our respective views on SOLID, Agile, IoC/DI, CI, etc. The really important point of an interview is determining whether there is a "fit" between myself and the company.

If there is a fit, then the fact that I haven’t been doing C# 4.0 for 5 years is irrelevant (ahem). If there’s no fit, the fact that you have a good dental plan is irrelevant.

So in the interest of saving time and skipping ahead to what matters, I decided to post my implementation of [FizzBuzz]. I used LINQ since I hadn’t seen that implementation, though I’m sure it’s out there.

    var numbers = from num in Enumerable.Range(1, 100)
        select num % 15 == 0 ? "FizzBuzz"
            : num % 5 == 0 ? "Buzz"
            : num % 3 == 0 ? "Fizz"
            : num.ToString();
 
    foreach (var num in numbers)
        Console.WriteLine(num);

That took about two minutes with a decent chunk of that spent firing up VS2010. Yes, I could have used a lambda for the Console.WriteLine, but I think the foreach is still more readable.

Great. I can write FizzBuzz. I also know all the access modifiers and I can use encapsulation and polymorphism in a sentence.

Now let’s move on. :-D

[FizzBuzz]:https://imranontech.com/2007/01/24/using-fizzbuzz-to-find-developers-who-grok-coding/

---
# comments begin here

