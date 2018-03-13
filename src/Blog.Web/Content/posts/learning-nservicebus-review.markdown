---
title: Learning NServiceBus Review
published: September 30, 2013
tags: 
---

I was contacted by Packt Publishing to review [Learning NServiceBus by David Boike][learning]. David is active in the NServiceBus community, so I was eager to see what he had put together. 

<figure>
  <img src="/content/posts/images/learning-nservicebus.png" alt="Cover photo of Learning NServiceBus by David Boike" />
  <figcaption>Learning NServiceBus by David Boike</figcaption>
</figure>

Using a messaging framework can be a daunting prospect for someone who is used to typical RPC / web service programming. In Learning NServiceBus, David has distilled the essential parts of getting messaging to work on NServiceBus into easily digestible bits. I consider myself a slow reader yet I was able to get through the book in a few hours. Also, as someone who is familiar with NServiceBus, I thought I might find the material boring. Instead, David presents it in a fun, engaging manner that had me happily swiping through the pages [kindle reader on iPad].

The book is divided into 8 chapters. One nice feature is that the first 6 chapters, which deal with code, have downloadable code samples that you can run on your own machine. There's no better way to understand code than to compile and run it. 

Chapter 1, "Getting on the IBus", goes over the basics of getting NServiceBus up and running. For many frameworks and platforms, getting started is make or break. NServiceBus has always impressed me as easy to get going. David takes us step by step through the process and explains what all the "bits" you download to your machine are and why they are there. He covers writing a basic message flow and starting it up in a console app. I like that the console app is pictured in color and the output messages are explained. 

All the code in the book is nicely formatted and easy to read. There is a great balance of showing you everything, explaining the relevant pieces and putting you at ease that things that were not explained, will be elaborated upon later in the book. In this way, each chapter builds upon the last.

I also felt there was a good balance of explaining just enough messaging theory to give context without dwelling on the ins and outs of SOA/DDD/TDD/ETC. There were a few points in the book where David simply referred to existing resources on the web to get further information. This is a good approach. While those things are essential to designing a message based architecture, they are not essential to understand how to use NServiceBus. The distinctions are not always clear, but David did a good job picking and choosing what to include. By pointing us to other resources, we're not left with that uneasy feeling that "there's something we don't know". Instead, we feel we have a good basic grasp of the necessary concepts and path to deepen our knowledge when necessary.

After each chapter I felt that I had received "just enough" information to start using the features described. While easy to digest, the book covers a lot of ground. Encryption, fault tolerance, logging, virtualization, monitoring, and scaling are all explored. 

Sagas, also known as long running processes or Process Managers, can be a challenge to understand at first glance. Chapter 6 focuses on Sagas and covers how you approach them from a technical perspective and from a business perspective. David reminds us that part of our jobs as software developers is understanding and educating the business, not simply typing code. 

A lot of tech books tend to be focused on programmers. Chapter 7 is focused on actually running NServiceBus from an administrators perspective. NServiceBus is opinionated about what things should be configured by programmers and which should be under the administrators control. Keeping this distinction front and center at all times help projects run successfully in production. It's easy to say "make everything configurable", but that often leads to unmaintainable software and strange bugs when no one can remember why a certain series of config incantations produce some unexpected behavior. Again, as a programmer, our job is not to write code and "throw it over the wall". We should be cognizant of how the code will be run in production and give the right levers to allow the operations folks to make informed decisions about the runtime environment without needing to call a developer.

The only annoyance I had in the book the use of exclamation points for emphasis. The first few times were fine, but after a while, it just pulled my focus out of the book!

All in all, as someone who has explained basic NServiceBus and Messaging on numerous occasions, I can now refer people to [Learning NServiceBus][learning] as a primer. Deep knowledge of a tool comes through usage. This book is a great way to start using a great tool.

[learning]:https://www.packtpub.com/application-development/learning-nservicebus

---
# comments begin here

- Email: "sandyp@packtpub.com"
  Message: "<p>Thanks for a detailed review of the book. Certainly looking forward to it.</p>"
  Name: "Sandy"
  When: "2013-10-03 09:12:04.000"