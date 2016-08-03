using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Blog.Web.Core;
using MarkdownSharp;

using static System.Environment;
using static System.Configuration.ConfigurationManager;

namespace Blog.Web.Infrastructure
{
    public class MarkdownSharpContentStorage
    {
        private readonly string _root;
        private readonly Markdown _markdown;
        private YamlDotNet.Serialization.Deserializer _yaml;

        public MarkdownSharpContentStorage(string root)
        {
            _root = root;
            _markdown = new Markdown();
            _yaml = new YamlDotNet.Serialization.Deserializer();

        }

        public PostViewModel Handle(PostViewModel message)
        {
            if (message == null) throw new ArgumentNullException("message");
            var model = ParseFile(message.FileName);
            message.Title = model.Title;
            message.PublishedAtCst = model.PublishedAtCst;
            message.Content = model.Content;
            message.Comments = model.Comments;
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
            var content = _markdown.Transform(parsed.Item2);
            var comments = _yaml.Deserialize<List<Comment>>(new StringReader(parsed.Item3)) ?? new List<Comment>();

            var model = new PostViewModel
            {
                Title = metadata["title"].FirstOrDefault(),
                Slug = filename.Replace(".markdown", string.Empty),
                Content = content,
                Comments = comments,
                PublishedAtCst = DateTime.Parse(metadata["published"].FirstOrDefault() ?? DateTime.MaxValue.ToString(CultureInfo.InvariantCulture)),
                FileName = filename
            };

            return model;
        }

        private Tuple<string, string, string> ParseText(string text)
        {
            //look for yaml document markers
            if (!text.StartsWith($"---{NewLine}")) return null;

            var endMetadata = text.IndexOf($"---{NewLine}", 3, StringComparison.Ordinal);
            if (endMetadata == -1) return null;

            var startComments = text.LastIndexOf($"---{NewLine}", StringComparison.Ordinal);
            
            var metadata = text.Substring(3, endMetadata - 3).Trim();
            var article = text.Substring(endMetadata + 3, startComments - endMetadata - 3).Trim().Replace("/content/posts/images", $@"{AppSettings["cdn::host"]}/content/posts/images");
            var comments = text.Substring(startComments + 3).Trim();

            return new Tuple<string, string, string>(metadata, article, comments);
        }

        private IEnumerable<KeyValuePair<string, string>> GetMetadata(string text)
        {
            var lines = text
                .Split(new[] {NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.Contains(":"));

            foreach (var line in lines)
            {
                var kv = line.Split(':');
                yield return new KeyValuePair<string, string>(kv[0], string.Join(":", kv.Skip(1)));
            }
        }
    }
}