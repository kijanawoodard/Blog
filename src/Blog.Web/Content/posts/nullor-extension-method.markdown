I find myself writing code like this a lot:

<script src="https://gist.github.com/4237737.js?file=nullor-problem.cs"></script>

I thought about adding an operator like ??? to go with ?? and ?, but you can’t do that in c# and it would probably be confusing to the next programmer anyway.

So how about an extension method to wrap that up:

<script src="https://gist.github.com/4237737.js?file=nullor-solution.cs"></script>

Not a lot less typing, but a bit clearer and you’re less likely to screw up.