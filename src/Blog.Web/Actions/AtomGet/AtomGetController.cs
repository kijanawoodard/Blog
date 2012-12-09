using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Web.Core;

namespace Blog.Web.Actions.AtomGet
{
    public class AtomGetController : Controller
    {
	    private readonly IPostVault _vault;

	    public AtomGetController(IPostVault vault)
		{
			_vault = vault;
		}

	    public ActionResult Execute()
	    {
            Response.ContentType = " application/atom+xml";
		    var model = _vault.Posts;
            return View(model);
        }

    }
}
