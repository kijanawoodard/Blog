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