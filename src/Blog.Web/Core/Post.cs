using System;
using System.Linq;

namespace Blog.Web.Core
{
    public class Post
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string FileName { get; set; }
        public DateTime PublishedAtCst { get; set; }
        public string[] Tags { get; set; }

        public Post()
        {
            Tags = new string[] {};
        }
    }

    public class PostViewModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string FileName { get; set; }
        public DateTime PublishedAtCst { get; set; }
        
        public string Summary
        {
            get
            {
                if (Content == null) return Title;
                var paragraphs = Content.Split(new[] {"</p>"}, StringSplitOptions.None);
                return paragraphs.Length == 0 ? Title : string.Join("</p>", paragraphs.Take(3));
            }
        }

        public string Content { get; set; }
    }
}