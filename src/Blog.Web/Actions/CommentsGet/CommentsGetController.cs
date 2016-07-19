using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YamlDotNet;

namespace Blog.Web.Actions.CommentsGet
{
    public class CommentsGetController : Controller
    {
        //
        // GET: /CommentsGet/

        public ActionResult Index(CommentsRequest request)
        {
            var root = System.Web.HttpContext.Current.Server.MapPath("~/Content/posts");
            var filename = $"{request.Slug}.comments.yaml";
            var path = Path.Combine(root, filename);
            var yaml = new YamlDotNet.Serialization.Deserializer();
            using (var reader = new StreamReader(path))
            {
                var comments = yaml.Deserialize<List<Comment>>(reader);
                var model = new CommentsViewModel { Comments = comments };
                return View(model);
            }
        }

        public class CommentsRequest
        {
            public string Slug { get; set; }
        }

        public class CommentsViewModel
        {
            public List<Comment> Comments { get; set; }
        }
    }

    public class Comment
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime When { get; set; }
        public string Message { get; set; }
    }
}
