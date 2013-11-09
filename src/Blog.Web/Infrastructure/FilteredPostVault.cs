using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Blog.Web.Actions.PostGet;
using Blog.Web.Core;

namespace Blog.Web.Infrastructure
{
	public class PostGetModule : IModule
	{
		public void Execute(IContainer container)
		{
			var root = HttpContext.Current.Server.MapPath("~/Content/posts");
			var mediator = container.Resolve<ISubscribeHandlers>();
			mediator.Subscribe<PostViewModel, PostViewModel>(message => new MarkdownContentStorage(root).Handle(message));
		}
	}

	public class FilteredPostVault
	{
		private IReadOnlyList<PostViewModel> ActivePosts { get; set; }
		private IReadOnlyList<PostViewModel> FuturePosts { get; set; }
		private IEnumerable<PostViewModel> AllPosts { get; set; }

		public FilteredPostVault(IMediator mediator)
		{
			var now = DateTime.UtcNow;
			var timezone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

			AllPosts =
				_posts
					.Select(post => new PostViewModel
					{
						Title = post.Title,
						Slug = post.Slug,
						FileName = post.FileName,
						PublishedAtCst = post.PublishedAtCst
					})
					.Select(mediator.Send<PostViewModel, PostViewModel>)
					.ToList();

			ActivePosts =
				AllPosts
					.OrderByDescending(x => x.PublishedAtCst)
					.Where(x => now >= TimeZoneInfo.ConvertTimeToUtc(x.PublishedAtCst, timezone))
					.ToList();

			FuturePosts = AllPosts.Except(ActivePosts).ToList();
		}

		public PostGetViewModel Handle(PostRequest message, PostGetViewModel result)
		{
			var post = ActivePosts.FirstOrDefault();
			if (message.Slug != null) post = AllPosts.FirstOrDefault(x => x.Slug.ToLower() == message.Slug.ToLower());
			if (post == null) return result; //Decision: don't throw, handle downstream as to what this means

			var previous = ActivePosts.OrderBy(x => x.PublishedAtCst).FirstOrDefault(x => x.PublishedAtCst > post.PublishedAtCst);
			var next = ActivePosts.FirstOrDefault(x => x.PublishedAtCst < post.PublishedAtCst);

			result.Post = post;
			result.Previous = previous;
			result.Next = next;
			
			return result;
		}

		public PostIndexViewModel Handle(PostIndexRequest message)
		{
			var result = new PostIndexViewModel();
			result.Active = ActivePosts;
			result.FuturePostCount = FuturePosts.Count;

			return result;
		}


		/*
		 * Features:
		 * 
		 *	Display posts csv
		 *	Xml format for archive?
		 *	hal extension/formatter
		 *	
		 * summaries
		 * home page - multiple entires
		 * link to archive?
		 * 
		 * Full posts in atom feed
		 * Update date vs published date
		 * Alternate urls in atom feed - json, xml, csv, html, phtml
		 * 
		 * conneg from content type
		 * 
		 * Automatic csv for complext types until you reach primitives property.property.property //http://stackoverflow.com/a/10929560/214073
		 */

		//blind men and the elephant
		//make your roles explicit - http://www.infoq.com/presentations/Making-Roles-Explicit-Udi-Dahan#anch41169

		//need a construct larger than a class, but smaller than a project - namespace?
		//private, protected, internal, public - need something else in between public and internal
		
		//reuse is coupling - if i can just get the interface right
		//generic repository - http://codebetter.com/gregyoung/2009/01/16/ddd-the-generic-repository/

		//avoid ginormous file and keep classes https://twitter.com/jbogard/status/387945793430495233

		/*
		 * http://www.sixmonthmba.com/2009/02/999ideas.html
		 * Video Menus
		 * RunBag.com
		 * 
		 */

		private readonly Post[] _posts =
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
				FileName = "foo-ifoo-is-an-antipattern.markdown",
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
				FileName = "violating-srp.markdown",
				PublishedAtCst = DateTime.Parse("October 11, 2013"),
			},
			new Post
			{
				Title = "Violating ISP with Constructor Injection",
				Slug = "violating-isp-with-constructor-injection",
				FileName = "violating-isp.markdown",
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
				FileName = "learning-nservicebus.markdown",
				PublishedAtCst = DateTime.Parse("September 30, 2013"),
			},
			new Post
			{
				Title = "Building My Own Blog",
				Slug = "building-a-blog-engine",
				FileName = "building-blog.markdown",
				PublishedAtCst = DateTime.Parse("December 8, 2012"),
			},
			new Post
			{
				Title = "Javascript UTC Datetime",
				Slug = "javascript-utc-datetime",
				FileName = "javascript-utc.markdown",
				PublishedAtCst = DateTime.Parse("June 05, 2012"),
			},
			new Post
			{
				Title = "Just use string Id for RavenDB",
				Slug = "just-use-string-id-for-ravendb",
				FileName = "raven-id.markdown",
				PublishedAtCst = DateTime.Parse("May 31, 2012"),
			},
			new Post
			{
				Title = "Seeking Density in Architectural Abstractions",
				Slug = "seeking-density-in-architectural-abstractions",
				FileName = "seeking-density.markdown",
				PublishedAtCst = DateTime.Parse("February 14, 2012"),
			},
			new Post
			{
				Title = "FubuMVC, Validation, and Re-Hydrating the View",
				Slug = "fubumvc-validation-and-re-hydrating-the-view",
				FileName = "rehydrating-views.markdown",
				PublishedAtCst = DateTime.Parse("February 12, 2012"),
			},
//			new Post
//			{
//				Title = "C# Partition List into List of Lists",
//				Slug = "c-partition-list-into-list-of-lists",
//				FileName = "partition-lists.markdown",
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
				FileName = "nullcheck-extension-method.markdown",
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
				FileName = "creating-using-directives.markdown",
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
				FileName = "gmail-spam.markdown",
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