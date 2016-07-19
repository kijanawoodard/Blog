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

[SO]:http://stackoverflow.com/questions/3773403/linq-partition-list-into-lists-of-8-members
[MoreLinq]:http://code.google.com/p/morelinq/source/browse/#svn/trunk/MoreLinq