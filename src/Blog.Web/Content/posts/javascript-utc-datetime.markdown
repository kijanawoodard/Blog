---
title: Javascript UTC Datetime
published: June 05, 2012
tags: 
---

I created a date in js like this: `new Date(2012, 0, 1, 8, 15, 0);`.

When I console.log it, I get this: `2012-01-01T14:15:00.000Z` (My timezone is CST).

The date is already converted to UTC and will convert back to local time for display. If you ship the date to js as UTC, it just works.

Once again, if you think you need to write a bunch of code to solve a common business problem, you're thinking incorrectly about the problem, the solution, or both.

---
# comments begin here

