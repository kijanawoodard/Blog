using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	//Foo - IFoo

	//why not put the dependency in the method
	//why can't an interface be declared for a method parameter

	//make your roles explicit - http://www.infoq.com/presentations/Making-Roles-Explicit-Udi-Dahan#anch41169

	//need a construct larger than a class, but smaller than a project - namespace?

	//private, protected, internal, public - need something else in between public and internal
	//reuse is coupling - if i can just get the interface right
	//generic repository - http://codebetter.com/gregyoung/2009/01/16/ddd-the-generic-repository/
	
	//avoid ginormous file and keep classes https://twitter.com/jbogard/status/387945793430495233
	public class ViolatingSrp : IPost
	{
		public string Title { get { return "Violating SRP with Constructor Injection"; } }
		public string Slug { get { return "violating-srp-with-constructor-injection"; } }
		public string FileName { get { return "violating-srp.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("October 11, 2013"); } }
	}

	public class ViolatingIsp : IPost
	{
		public string Title { get { return "Violating ISP with Constructor Injection"; } }
		public string Slug { get { return "violating-isp-with-constructor-injection"; } }
		public string FileName { get { return "violating-isp.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("October 10, 2013"); } }
	}

	public class QuestioningIocContainers : IPost
	{ 
		public string Title { get { return "Questioning IoC Containers"; } }
		public string Slug { get { return "questioning-ioc-containers"; } }
		public string FileName { get { return "questioning-ioc-containers.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("October 9, 2013"); } }
	}

	public class DotnetDemon : IPost
	{
		public string Title { get { return ".Net Demon"; } }
		public string Slug { get { return "dotnet-demon"; } }
		public string FileName { get { return "dotnet-demon.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("October 8, 2013"); } }
	}

	public class LearningNServiceBus : IPost
	{
		public string Title { get { return "Learning NServiceBus Review"; } }
		public string Slug { get { return "learning-nservicebus-review"; } }
		public string FileName { get { return "learning-nservicebus.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("September 30, 2013"); } }
	}

	public class BuildingABlog : IPost
	{
		public string Title { get { return "Building My Own Blog"; } }
		public string Slug { get { return "building-a-blog-engine"; } }
		public string FileName { get { return "building-blog.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("December 8, 2012"); } }
	}

	public class JavascriptUtc : IPost
	{
		public string Title { get { return "Javascript UTC Datetime"; } }
		public string Slug { get { return "javascript-utc-datetime"; } }
		public string FileName { get { return "javascript-utc.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("June 05, 2012"); } }
	}

	public class RavenId : IPost
	{
		public string Title { get { return "Just use string Id for RavenDB"; } }
		public string Slug { get { return "just-use-string-id-for-ravendb"; } }
		public string FileName { get { return "raven-id.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 31, 2012"); } }
	}

	public class SeekingDensity : IPost
	{
		public string Title { get { return "Seeking Density in Architectural Abstractions"; } }
		public string Slug { get { return "seeking-density-in-architectural-abstractions"; } }
		public string FileName { get { return "seeking-density.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("February 14, 2012"); } }
	}

	public class RehydratingViews : IPost
	{
		public string Title { get { return "FubuMVC, Validation, and Re-Hydrating the View"; } }
		public string Slug { get { return "fubumvc-validation-and-re-hydrating-the-view"; } }
		public string FileName { get { return "rehydrating-views.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("February 12, 2012"); } }
	}

	public class PartitionLists
	{
		public string Title { get { return "C# Partition List into List of Lists"; } }
		public string Slug { get { return "c-partition-list-into-list-of-lists"; } }
		public string FileName { get { return "partition-lists.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("October 16, 2010"); } }
	}

	public class SellingValueForMoney : IPost
	{
		public string Title { get { return "Selling Value for Money"; } }
		public string Slug { get { return "selling-value-for-money"; } }
		public string FileName { get { return "selling-value-for-money.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 20, 2010"); } }
	}

	public class NullOrExtensionMethod : IPost
	{
		public string Title { get { return "NullOr Extension Method"; } }
		public string Slug { get { return "nullor-extension-method"; } }
		public string FileName { get { return "nullor-extension-method.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 19, 2010"); } }
	}

	public class NullCheckExtensionMethod : IPost
	{
		public string Title { get { return "Null Check with Extension Methods"; } }
		public string Slug { get { return "null-check-with-extension-methods"; } }
		public string FileName { get { return "nullcheck-extension-method.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 18, 2010"); } }
	}

	public class AvoidNegativeBranching : IPost
	{
		public string Title { get { return "Avoiding Negative Branching Tests"; } }
		public string Slug { get { return "avoiding-negative-branching-tests"; } }
		public string FileName { get { return "avoiding-negative-branching-tests.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 17, 2010"); } }
	}

	public class CoolExtensionMethods : IPost
	{
		public string Title { get { return "Cool Feature of Extension Methods"; } }
		public string Slug { get { return "cool-feature-of-extension-methods"; } }
		public string FileName { get { return "cool-feature-of-extension-methods.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 16, 2010"); } }
	}

	public class VisualStudioCodeSnippets : IPost
	{
		public string Title { get { return "Visual Studio Code Snippets"; } }
		public string Slug { get { return "visual-studio-code-snippets"; } }
		public string FileName { get { return "visual-studio-code-snippets.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 15, 2010"); } }
	}

	public class CreatingUsingDirectives : IPost
	{
		public string Title { get { return "Quickly Creating Using Namespace Directives"; } }
		public string Slug { get { return "quickly-creating-using-namespace-directives"; } }
		public string FileName { get { return "creating-using-directives.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 14, 2010"); } }
	}

	public class ScriptSharp : IPost
	{
		public string Title { get { return "Thoughts on Script#"; } }
		public string Slug { get { return "thoughts-on-script-sharp"; } }
		public string FileName { get { return "thoughts-on-script-sharp.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 12, 2010"); } }
	}

	public class InDefenseOfBlub : IPost
	{
		public string Title { get { return "In Defense of Blub"; } }
		public string Slug { get { return "in-defense-of-blub"; } }
		public string FileName { get { return "in-defense-of-blub.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 11, 2010"); } }
	}

	public class EnumsSuck : IPost
	{
		public string Title { get { return "Why Enums Suck"; } }
		public string Slug { get { return "why-enums-suck"; } }
		public string FileName { get { return "why-enums-suck.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 10, 2010"); } }
	}

	public class FizzBuzzPlusPlus : IPost
	{
		public string Title { get { return "FizzBuzz++"; } }
		public string Slug { get { return "fizzbuzz-plus-plus"; } }
		public string FileName { get { return "fizzbuzz-plus-plus.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 9, 2010 2:00"); } }
	}

	public class EnumerableEach : IPost
	{
		public string Title { get { return "IEnumerable Each()"; } }
		public string Slug { get { return "ienumerable-each"; } }
		public string FileName { get { return "ienumerable-each.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 9, 2010 1:00"); } }
	}

	public class AvoidingFizzBuzz : IPost
	{
		public string Title { get { return "Avoiding FizzBuzz"; } }
		public string Slug { get { return "avoiding-fizzbuzz"; } }
		public string FileName { get { return "avoiding-fizzbuzz.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("May 9, 2010"); } }
	}

	public class GmailSpam : IPost
	{
		public string Title { get { return "Gmail’s Spam Criteria is Stringent"; } }
		public string Slug { get { return "gmails-spam-criteria-is-stringent"; } }
		public string FileName { get { return "gmail-spam.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("January 11, 2010"); } }
	}

	public class TekPubRocks : IPost
	{
		public string Title { get { return "TekPub Rocks!"; } }
		public string Slug { get { return "tekpub-rocks"; } }
		public string FileName { get { return "tekpub-rocks.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("January 10, 2010"); } }
	}
}