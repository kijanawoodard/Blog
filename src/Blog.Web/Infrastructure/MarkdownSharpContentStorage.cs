using System;
using System.IO;
using Blog.Web.Core;
using MarkdownSharp;

namespace Blog.Web.Infrastructure
{
    public class MarkdownSharpContentStorage
    {
        private readonly string _root;
        private readonly Markdown _markdown;

        public MarkdownSharpContentStorage(string root)
        {
            _root = root;
            _markdown = new Markdown();

        }

        public PostViewModel Handle(PostViewModel message)
        {
            if (message == null) throw new ArgumentNullException("message");
            message.Content = GetContent(message.FileName);
            return message;
        }

        private string GetContent(string filename)
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