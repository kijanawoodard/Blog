using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            var model = ParseFile(message.FileName);
            message.Title = model.Title;
            message.Content = model.Content;
            message.PublishedAtCst = model.PublishedAtCst;
            return message;
        }
        
        public IEnumerable<PostViewModel> GetAll()
        {
            var files = Directory.EnumerateFiles(_root, "*.markdown");
            return files.Select(Path.GetFileName).Select(ParseFile).Where(x => x != null);
        } 

        private PostViewModel ParseFile(string filename)
        {
            var path = Path.Combine(_root, filename);
            if (!File.Exists(path)) return null;

            var text = File.ReadAllText(path);
            var parsed = ParseText(text);
            if (parsed == null) return null;

            var metadata = GetMetadata(parsed.Item1).ToLookup(x => x.Key, x => x.Value);
            var html = _markdown.Transform(parsed.Item2);

            var model = new PostViewModel
            {
                Title = metadata["title"].FirstOrDefault(),
                Slug = filename.Replace(".markdown", string.Empty),
                PublishedAtCst = DateTime.Parse(metadata["published"].FirstOrDefault() ?? DateTime.MaxValue.ToString(CultureInfo.InvariantCulture)),
                Content = html,
                FileName = filename
            };

            return model;
        }

        private Tuple<string, string> ParseText(string text)
        {
            //look for two yaml lines
            if (!text.StartsWith("---")) return null;

            var endMetadata = text.IndexOf("---", 3, StringComparison.Ordinal);
            if (endMetadata == -1) return null;

            var metadata = text.Substring(3, endMetadata - 3).Trim();
            var article = text.Substring(endMetadata + 3).Trim();
            return new Tuple<string, string>(metadata, article);
        }

        private IEnumerable<KeyValuePair<string, string>> GetMetadata(string text)
        {
            var lines = text
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.Contains(":"));

            foreach (var line in lines)
            {
                var kv = line.Split(':');
                yield return new KeyValuePair<string, string>(kv[0], kv[1]);
            }
        }
    }
}