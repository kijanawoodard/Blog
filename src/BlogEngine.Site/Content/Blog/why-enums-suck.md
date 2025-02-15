---
title: Why Enums Suck
published: May 10, 2010
tags: 
---

I inherited some code at work that made use of enums. I happily continued the pattern in order to get the job done considering the tight deadline. My spidey sense kept tingling telling me something was wrong, but I couldn’t quite put my finger on it.

The code I started with was pretty standard stuff.

There was an enum:

    enum CarType
    {
        Slow,
        Fast,
        Lightning
    }

There was a class that used the enum:

    class Car
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public CarType CarType { get; set; }
 
        public Car() { Id = Guid.NewGuid(); }
    }        

There was some List creation:

    var cars = new List<Car>()
    {
        new Car() { Name = "Yugo", CarType = CarType.Slow },
        new Car() { Name = "M3", CarType = CarType.Fast },
        new Car() { Name = "Tesla Roadster", CarType = CarType.Lightning }
    };
            
And then there was branching logic on the enum. This is where the trouble began:

    foreach (var car in cars)
    {
        switch (car.CarType)
        {
            case CarType.Slow:
                DoSlowCarStuff();           
                break;
            case CarType.Fast:
                DoFastCarStuff();
                break;
            case CarType.Lightning:
                DoLightningCarStuff();
                break;
            default:
                break;
        }
    }        

This code was smelly and I was adding more of it. I really didn’t understand what I didn’t like, but I wanted to do something different.

I decided to use extension methods.

    public static class CarTypeExtensionMethods
    {
        public static void DoCarStuff(this CarType type)
        {
            switch (type)
            {
                case CarType.Slow:
                    DoSlowCarStuff();
                    break;
                case CarType.Fast:
                    DoFastCarStuff();
                    break;
                case CarType.Lightning:
                    DoLightningCarStuff();
                    break;
                default:
                    break;
            }
        }
 
        static void DoSlowCarStuff() { }
        static void DoFastCarStuff() { }
        static void DoLightningCarStuff() { }
    }        

So now my consuming code looked like this:

    foreach (var car in cars)
        car.CarType.DoCarStuff(); 
           
Ahhhh. Now that’s bliss. All the car type junk was packaged together and the calling code is dead simple.

But something still felt…wrong. The big “switches” were all gone, but I still had some “if (carType ==” statements lying around. I could put those in the extension methods, but that wasn’t really the root issue.

I went to the Big G and typed something like “c# alternatives to enums”. Somewhere along the line I stumbled on this post about comparing c# enums to java enums.

At first, I thought, this looks like a ton more code to write for little gain. But it felt like the right direction. I decided to just write it and see what happened.

    class CarType
    {
        public static readonly CarType Slow = new CarType()
        {
            _display = "Slow",
            _dostuff = () =>
            {
                //do slow car stuff
            }
        };
 
        public static readonly CarType Fast = new CarType()
        {
            _display = "Fast",
            _dostuff = () =>
            {
                //do fast car stuff
            }
        };
 
        public static readonly CarType Lightning = new CarType()
        {
            _display = "Lightning",
            _dostuff = () =>
            {
                //do lightning car stuff
            }
        };
 
        public override string ToString()
        {
            return _display;
        }
 
        public void DoCarStuff()
        {
           _dostuff();
        }
        private CarType() { }
        private string _display;
        private Action _dostuff;
    }        

And suddenly the real problem with the original code was obvious. Switch/If blocks were littered everywhere through the program. If you added a new CarType, you’d have to hunt through the entire application updating the switch/if logic.

The extension method class was better in that the code was all in one class, but you still had to go through and update it all.

Now, when you create a new “enum” type, all the logic is done right there. Even typing up this blog post I smiled when I didn’t have to change my Car class or the consuming code that called DoCarStuff(). I can add CarTypes at will, knowing I don’t have to change any other code.

Enums are still useful when all they do is identity. As soon as you start branching on enums, switch to a class. You’ll thank me later.

So by now, you might be thinking, congratulations, you’ve discovered the strategy pattern. I get that. However, I find it useful to think about solving concrete problems like getting rid of branching code on enums by using proper classes. It’s the same thing, but if I just said, “use the strategy pattern”, a lot of people, myself included, would leave the blog post less informed.

Finally, I know some of you might think that this code is terrible:

    car.CarType.DoCarStuff();        

I realize that Car should probably define DoCarStuff and not expose it’s CarType, but this was the first example I could think of and I figured I’d write the post instead of trying to think of the perfect example.

Thoughts?

---
# comments begin here

- Email: "joey@joeyguerra.com"
  Message: "<p>The strategy pattern was exactly what I was thinking. And I totally agree with you on that point, I learned something. Thanks.</p>"
  Name: "Joey Guerra"
  When: "2010-05-11 07:41:08.000"
- Email: "abfab@querty.com"
  Message: "<p>Congratulations, you've re-invented inheritance!</p>"
  Name: "F"
  When: "2012-03-23 13:45:48.000"
- Email: "kijanawoodard@wyldeye.com"
  Message: "<p>Congratulations, you (nearly) understood the post!</p><p>I didn't \"re-invent inheritance\", I'm *utilizing* classes to solve a particular type of problem instead of using magic strings or enums. The whole point of the post is that you should bias towards leaning on OO instead of enums.</p>"
  Name: "kijana"
  When: "2012-03-30 20:55:47.000"
- Email: "mohamed.elmalky@gmail.com"
  Message: "<p>Thank you for this gem of an idea. Makes code much cleaner.</p>"
  Name: "Mohamed Elmalky"
  When: "2013-01-14 20:27:29.000"
- Email: "y543@yahoo.com"
  Message: "<p>Thanks, you helped me make my argument complete.</p>"
  Name: "Yuri Vaillant"
  When: "2013-01-22 09:46:22.000"
- Email: "diogo.filipe.acastro@gmail.com"
  Message: "<p>1) you overlooked the simplest and best solution for your scenario: inheritance. You should extend Car and let each class have its own implementation of DoCarStuff <br>2) that's not the strategy pattern, and that breaks about a dozen of design principles, like the open closed principle and the law of Demeter for example.<br>3) Enums don't suck. Whoever wrote the original code clearly doesn't know *when* to use them - he's the problem.</p>"
  Name: "Diogo Castro"
  When: "2013-09-27 19:38:56.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Three years later, I'm still not thrilled with the code above, but the general idea is the same: don't use enums to drive behavior, use classes. I think we agree on that much.</p>"
  Name: "Kijana Woodard"
  When: "2013-09-27 20:20:18.000"
- Email: "diogo.filipe.acastro@gmail.com"
  Message: "<p>Definitely! That's a good lesson to take from this. I also agree with you when you say that enums are good for identification. \"LogLevel\" is a classical example.</p>"
  Name: "Diogo Castro"
  When: "2013-09-29 21:36:28.000"
- Email: "willmotil@live.com"
  Message: "<p>well this looks like the place to rant  enums not only suck they go back in time to the days of<br>ill return whatever i feel like, after fiddling with this <br>i just decided to write my own class its not even worth the time, its broken and backwards</p><p><pre><code>// first how can we infer a return type i cant even manually new<br>// myenum[] ve = myenum[4]{};<br />// ya ok whatever<br>// so get values really gets myenums array with some random spoofed names in it now ?<br /><br />var ve = (myenum[])Enum.GetValues(typeof(myenum));<br>for (int i = 0; i &lt; ve.Length; i++)<br>{<br>    Console.Write(ve[i] + \" \");<br>}<br>Console.WriteLine(\"\\n so do i hate hate you no thats just what it assumed\");<br>// so now to undo what they did , you have to do it like this cause <br>// you need a totally separate method to actually get back what you put in<br>var ne = Enum.GetNames(typeof(myenum));<br>for (int i = 0; i &lt; ne.Length; i++)<br>{<br>    Console.Write(ne[i] + \" \");<br>}<br>// output<br>//I hate hate you<br>// no<br>//I love hate you<br></pre></code>// and god forbid you want to generically pass it to a method(enum t, forget it</p>"
  Name: "will"
  When: "2014-06-17 05:17:18.000"
- Email: "willmotil@live.com"
  Message: "<p><pre><code>[Flags]<br>public enum myenum<br>{<br>    I = 0,<br>    love = 1,<br>    hate = 1,<br>    you = 2<br>}</code></pre></p>"
  Name: "will"
  When: "2014-06-17 05:21:20.000"
- Email: "willmotil@live.com"
  Message: "<p>oh ya i almost forgot you dont declare it static you cant<br>but you can call it in a static method from outside the method <br>even though you cant pass it to a method<br>how is any of that sensable or clear<br>and this is recommended really</p>"
  Name: "will"
  When: "2014-06-17 05:30:23.000"
- Email: "disqus@wyldeye.com"
  Message: "<p>Indeed. F# discriminated unions look to be a better alternative. <a href=\"https://fsharpforfunandprofit.com/posts/discriminated-unions/\" rel=\"nofollow\">https://fsharpforfunandprofit.c...</a></p>"
  Name: "Kijana Woodard"
  When: "2014-06-17 14:44:30.000"
- Email: "lingmaaki@gmail.com"
  Message: "<p>More about c# enum...<a href=\"https://csharp.net-informations.com/statements/enum.htm\" rel=\"nofollow\">C# Enum</a></p><p>Ling</p>"
  Name: "ling maaki"
  When: "2014-07-08 06:18:06.000"