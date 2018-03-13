[Edit: See the comments to see why this post is totally unnecessary. Thanks Matt!]

I found myself in an odd situation. I had a List of objects that I needed to break into a List of Lists to make display a little more, well, sane. I did a web search and found a couple entries detailing [how to break a list into fixed sized chunks][SO].

However, my situation was such that each partition could vary in size.

I have a class that looks roughly like this:

    private class Score
    {
        public int ScoreTypeId { get; set; }
        public double Score { get; set; }
        public int DivisionId { get; set; }
    }

Some divisions have 11 “ScoreTypes”, some have 10, some have 7, etc. So I needed a way to break up my single list (the result of a db query) into chunks to give to my View code.

I came up with this:

    public static class IEnumerableExtender
    {
        public static IEnumerable<IEnumerable<T>> Partition<T, TResult>(
                 this IEnumerable<T> list, Func<T, TResult> partition)
        {
            var elements = list.Select(partition).Distinct();
            foreach (var item in elements)
            {
                yield return list.Where(x => partition(x).Equals(item));
            }
        }
    }

    //usage:
    var final = list.Partition(x => x.DivisionId );


I’m pretty happy with this. I played around partitioning by different elements and even a bit with partitioning by multiple elements. It kept on producing expected results. 

p.s.

That [MoreLinq] project by Jon Skeet mentioned in the StackOverflow post looks interesting. I didn’t get a chance to go through it yet.

[SO]:https://stackoverflow.com/questions/3773403/linq-partition-list-into-lists-of-8-members
[MoreLinq]:https://code.google.com/p/morelinq/source/browse/#svn/trunk/MoreLinq

---
# comments begin here

- Email: "luv2code+kijanassite@gmail.com"
  Message: "<p>Seems like GroupBy would work in this scenario.  How is this different?</p>"
  Name: "Matt T"
  When: "2010-10-17 08:28:55.000"
- Email: "luv2code+kijanassite@gmail.com"
  Message: "<p>Wouldn't GroupBy on DivisionId work in this scenario?<br>Here's a linqpad query to illustrate what I mean:<br>void Main()<br>{</p><p>var list = Enumerable.Range(0, 9);</p><p>var groups = from x in list<br>group x by x.ModByTwo() into itemgroup<br>select itemgroup;<br>groups.Dump();<br>}</p><p>// Define other methods and classes here<br>public static class IntExtension<br>{<br>public static int ModByTwo(this int input){<br>return input % 2;<br>}<br>}</p>"
  Name: "Matt T"
  When: "2010-10-17 08:40:07.000"
- Email: "kijanawoodard@wyldeye.com"
  Message: "<p>I don't think so. I may not fully grok the linq group by, but doesn't it flatten the records similar to a sql group by? I still needed the individual records, but I needed them in sets based on their DivisionId, hence the List of Lists.</p><p>A ModByTwo-like function would require knowledge of how many items to stick in each sub lists which we don't know until runtime, but I think you were just using that for illustration.</p>"
  Name: "kijana"
  When: "2010-10-17 10:01:31.000"
- Email: "kijanawoodard@wyldeye.com"
  Message: "<p>Well blow me down. You're right. I had to go try it out.</p><p>All I had to was list.GroupBy(x =&gt; x.DivisionId);</p><p>I was confused because it returns IGrouping. I thought I had to do something special with it. I bound it to my repeater and, blam, everything worked without the partitioning logic. No extension methods necessary.</p><p>The Linq group by IS different from sql. Nice.</p>"
  Name: "kijana"
  When: "2010-10-17 10:14:13.000"