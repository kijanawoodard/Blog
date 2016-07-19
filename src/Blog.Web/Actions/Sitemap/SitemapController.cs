using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Web.Actions.Sitemap
{
    public class SitemapController : Controller
    {
        public ActionResult Index()
        {
            return File("content/sitemap.xml", "application/xml");
        }

    }
}
