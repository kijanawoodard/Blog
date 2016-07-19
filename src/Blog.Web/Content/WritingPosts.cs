using System;
using System.Collections.Generic;
using Blog.Web.Core;

namespace Blog.Web.Content
{
	public static class WritingPosts
	{
		public static IReadOnlyCollection<Post> Posts
		{
			get
			{
				return new[]
				{
					new Post
					{
						Title = "Asp.net MVC Content Negotiation",
						Slug = "asp-net-mvc-content-negotiation",
						FileName = "asp-net-mvc-content-negotiation.markdown",
						PublishedAtCst = DateTime.Parse("November 08, 2013"),
					},
					new Post
					{
						Title = "A Tale of Scope Creep",
						Slug = "a-tale-of-scope-creep",
						FileName = "a-tale-of-scope-creep.markdown",
						PublishedAtCst = DateTime.Parse("November 05, 2013"),
					},
					new Post
					{
						Title = "Vessel Modules",
						Slug = "vessel-modules",
						FileName = "vessel-modules.markdown",
						PublishedAtCst = DateTime.Parse("November 01, 2013"),
					},
					new Post
					{
						Title = "Introducing Vessel",
						Slug = "introducing-vessel",
						FileName = "introducing-vessel.markdown",
						PublishedAtCst = DateTime.Parse("October 31, 2013"),
					},
					new Post
					{
						Title = "Introducing Liaison",
						Slug = "introducing-liaison",
						FileName = "introducing-liaison.markdown",
						PublishedAtCst = DateTime.Parse("October 25, 2013"),
					},
					new Post
					{
						Title = "Introducing Nimbus",
						Slug = "introducing-nimbus",
						FileName = "introducing-nimbus.markdown",
						PublishedAtCst = DateTime.Parse("October 24, 2013"),
					},
					new Post
					{
						Title = "Contact",
						Slug = "contact",
						FileName = "contact.markdown",
						PublishedAtCst = DateTime.Parse("April 17, 1974"),
					},
					new Post
					{
						Title = "Creating a ReSharper Macro",
						Slug = "creating-a-resharper-macro",
						FileName = "creating-a-resharper-macro.markdown",
						PublishedAtCst = DateTime.Parse("Thursday, October 17, 2013"),
					},
					new Post
					{
						Title = "Constructor Injection is Partial Application",
						Slug = "constructor-injection-is-partial-application",
						FileName = "constructor-injection-is-partial-application.markdown",
						PublishedAtCst = DateTime.Parse("October 16, 2013"),
					},
					new Post
					{
						Title = "Interface Inversion",
						Slug = "interface-inversion",
						FileName = "interface-inversion.markdown",
						PublishedAtCst = DateTime.Parse("October 15, 2013"),
					},
					new Post
					{
						Title = "Foo: IFoo is an Anti-Pattern",
						Slug = "foo-ifoo-is-an-anti-pattern",
						FileName = "foo-ifoo-is-an-anti-pattern.markdown",
						PublishedAtCst = DateTime.Parse("October 14, 2013"),
					},
					new Post
					{
						Title = "How to Wake Up Your Kids",
						Slug = "how-to-wake-up-your-kids",
						FileName = "how-to-wake-up-your-kids.markdown",
						PublishedAtCst = DateTime.Parse("October 12, 2013"),
						Tags = new[] {"missives"},
					},
					new Post
					{
						Title = "Violating SRP with Constructor Injection",
						Slug = "violating-srp-with-constructor-injection",
						FileName = "violating-srp-with-constructor-injection.markdown",
						PublishedAtCst = DateTime.Parse("October 11, 2013"),
					},
					new Post
					{
						Title = "Violating ISP with Constructor Injection",
						Slug = "violating-isp-with-constructor-injection",
						FileName = "violating-isp-with-constructor-injection.markdown",
						PublishedAtCst = DateTime.Parse("October 10, 2013"),
					},
					new Post
					{
						Title = "Questioning IoC Containers",
						Slug = "questioning-ioc-containers",
						FileName = "questioning-ioc-containers.markdown",
						PublishedAtCst = DateTime.Parse("October 9, 2013"),
					},
					new Post
					{
						Title = ".Net Demon",
						Slug = "dotnet-demon",
						FileName = "dotnet-demon.markdown",
						PublishedAtCst = DateTime.Parse("October 8, 2013"),
						Tags = new[] {"little things"},
					},
					new Post
					{
						Title = "Learning NServiceBus Review",
						Slug = "learning-nservicebus-review",
						FileName = "learning-nservicebus-review.markdown",
						PublishedAtCst = DateTime.Parse("September 30, 2013"),
					},
					new Post
					{
						Title = "Building My Own Blog",
						Slug = "building-a-blog-engine",
						FileName = "building-a-blog-engine.markdown",
						PublishedAtCst = DateTime.Parse("December 8, 2012"),
					},
					new Post
					{
						Title = "Javascript UTC Datetime",
						Slug = "javascript-utc-datetime",
						FileName = "javascript-utc-datetime.markdown",
						PublishedAtCst = DateTime.Parse("June 05, 2012"),
					},
					new Post
					{
						Title = "Just use string Id for RavenDB",
						Slug = "just-use-string-id-for-ravendb",
						FileName = "just-use-string-id-for-ravendb.markdown",
						PublishedAtCst = DateTime.Parse("May 31, 2012"),
					},
					new Post
					{
						Title = "Seeking Density in Architectural Abstractions",
						Slug = "seeking-density-in-architectural-abstractions",
						FileName = "seeking-density-in-architectural-abstractions.markdown",
						PublishedAtCst = DateTime.Parse("February 14, 2012"),
					},
					new Post
					{
						Title = "FubuMVC, Validation, and Re-Hydrating the View",
						Slug = "fubumvc-validation-and-re-hydrating-the-view",
						FileName = "fubumvc-validation-and-re-hydrating-the-view.markdown",
						PublishedAtCst = DateTime.Parse("February 12, 2012"),
					},
//			new Post
//			{
//				Title = "C# Partition List into List of Lists",
//				Slug = "c-partition-list-into-list-of-lists",
//				FileName = "c-partition-list-into-list-of-lists.markdown",
//				PublishedAtCst = DateTime.Parse("October 16, 2010"),
//				Tags = new string[] {},
//			},
					new Post
					{
						Title = "Selling Value for Money",
						Slug = "selling-value-for-money",
						FileName = "selling-value-for-money.markdown",
						PublishedAtCst = DateTime.Parse("May 20, 2010"),
					},
					new Post
					{
						Title = "NullOr Extension Method",
						Slug = "nullor-extension-method",
						FileName = "nullor-extension-method.markdown",
						PublishedAtCst = DateTime.Parse("May 19, 2010"),
					},
					new Post
					{
						Title = "Null Check with Extension Methods",
						Slug = "null-check-with-extension-methods",
						FileName = "null-check-with-extension-methods.markdown",
						PublishedAtCst = DateTime.Parse("May 18, 2010"),
					},
					new Post
					{
						Title = "Avoiding Negative Branching Tests",
						Slug = "avoiding-negative-branching-tests",
						FileName = "avoiding-negative-branching-tests.markdown",
						PublishedAtCst = DateTime.Parse("May 17, 2010"),
					},
					new Post
					{
						Title = "Cool Feature of Extension Methods",
						Slug = "cool-feature-of-extension-methods",
						FileName = "cool-feature-of-extension-methods.markdown",
						PublishedAtCst = DateTime.Parse("May 16, 2010"),
					},
					new Post
					{
						Title = "Visual Studio Code Snippets",
						Slug = "visual-studio-code-snippets",
						FileName = "visual-studio-code-snippets.markdown",
						PublishedAtCst = DateTime.Parse("May 15, 2010"),
					},
					new Post
					{
						Title = "Quickly Creating Using Namespace Directives",
						Slug = "quickly-creating-using-namespace-directives",
						FileName = "quickly-creating-using-namespace-directives.markdown",
						PublishedAtCst = DateTime.Parse("May 14, 2010"),
					},
					new Post
					{
						Title = "Thoughts on Script#",
						Slug = "thoughts-on-script-sharp",
						FileName = "thoughts-on-script-sharp.markdown",
						PublishedAtCst = DateTime.Parse("May 12, 2010"),
					},
					new Post
					{
						Title = "In Defense of Blub",
						Slug = "in-defense-of-blub",
						FileName = "in-defense-of-blub.markdown",
						PublishedAtCst = DateTime.Parse("May 11, 2010"),
					},
					new Post
					{
						Title = "Why Enums Suck",
						Slug = "why-enums-suck",
						FileName = "why-enums-suck.markdown",
						PublishedAtCst = DateTime.Parse("May 10, 2010"),
					},
					new Post
					{
						Title = "FizzBuzz++",
						Slug = "fizzbuzz-plus-plus",
						FileName = "fizzbuzz-plus-plus.markdown",
						PublishedAtCst = DateTime.Parse("May 9, 2010 2:00"),
					},
					new Post
					{
						Title = "IEnumerable Each()",
						Slug = "ienumerable-each",
						FileName = "ienumerable-each.markdown",
						PublishedAtCst = DateTime.Parse("May 9, 2010 1:00"),
					},
					new Post
					{
						Title = "Avoiding FizzBuzz",
						Slug = "avoiding-fizzbuzz",
						FileName = "avoiding-fizzbuzz.markdown",
						PublishedAtCst = DateTime.Parse("May 9, 2010"),
					},
					new Post
					{
						Title = "Gmail’s Spam Criteria is Stringent",
						Slug = "gmails-spam-criteria-is-stringent",
						FileName = "gmails-spam-criteria-is-stringent.markdown",
						PublishedAtCst = DateTime.Parse("January 11, 2010"),
					},
					new Post
					{
						Title = "TekPub Rocks!",
						Slug = "tekpub-rocks",
						FileName = "tekpub-rocks.markdown",
						PublishedAtCst = DateTime.Parse("January 10, 2010"),
					}
				};
			}
		}
	}
}