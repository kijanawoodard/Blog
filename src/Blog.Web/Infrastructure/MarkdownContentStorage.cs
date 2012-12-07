using System.IO;
using Blog.Web.Core;
using MarkdownSharp;

namespace Blog.Web.Infrastructure
{
	public class MarkdownContentStorage : IContentStorage
	{
		private readonly string _root;
		private readonly Markdown _markdown;

		public MarkdownContentStorage(string root)
		{
			_root = root;
			_markdown = new Markdown();

		}

		public string GetContent(string filename)
		{
			filename = Path.Combine(_root, filename);
			if (!File.Exists(filename)) return string.Empty;

			string fileContents;

			using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
			using (var streamReader = new StreamReader(fileStream))
			{
				fileContents = streamReader.ReadToEnd();
			}

			return _markdown.Transform(fileContents);
		}
	}
}