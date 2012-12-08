using System;
using Blog.Web.Core;

namespace Blog.Web.Models
{
	public class RehydratingViews : IPost
	{
		public string Title { get { return "FubuMVC, Validation, and Re-Hydrating the View"; } }
		public string Slug { get { return "fubumvc-validation-and-re-hydrating-the-view"; } }
		public string FileName { get { return "rehydrating-views.markdown"; } }
		public DateTime PublishedAtCst { get { return DateTime.Parse("February 12, 2012"); } }
	}

	public class PartitionLists : IPost
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
}