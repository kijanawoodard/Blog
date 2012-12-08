[Edit: See the comments to see why this post is totally unnecessary. Thanks Matt T!]

I found myself in an odd situation. I had a List of objects that I needed to break into a List of Lists to make display a little more, well, sane. I did a web search and found a couple entries detailing [how to break a list into fixed sized chunks][SO].

However, my situation was such that each partition could vary in size.

I have a class that looks roughly like this:

<script src="https://gist.github.com/4237737.js?file=partition-lists-score.cs"></script>

Some divisions have 11 “ScoreTypes”, some have 10, some have 7, etc. So I needed a way to break up my single list (the result of a db query) into chunks to give to my View code.

I came up with this:

<script src="https://gist.github.com/4237737.js?file=partition-lists-enumerable.cs"></script>

I’m pretty happy with this. I played around partitioning by different elements and even a bit with partitioning by multiple elements. It kept on producing expected results. 

p.s.

That [MoreLinq] project by Jon Skeet mentioned in the StackOverflow post looks interesting. I didn’t get a chance to go through it yet.

[SO]:http://stackoverflow.com/questions/3773403/linq-partition-list-into-lists-of-8-members
[MoreLinq]:http://code.google.com/p/morelinq/source/browse/#svn/trunk/MoreLinq