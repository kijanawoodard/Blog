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

Notice I’m still trying to decompose statements into rough single responsibility. I’m still going for readability/maintainability over trying to minimize statement count. At the same time, I always strive to reduce typing by staying pretty DRY. It’s a balancing act.

Thoughts?

[Avoiding FizzBuzz]:http://kijanawoodard.com/avoiding-fizzbuzz/
[enumerable]:http://kijanawoodard.com/ienumerable-each/