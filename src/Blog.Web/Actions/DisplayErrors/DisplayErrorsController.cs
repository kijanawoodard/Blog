using System;
using System.Net;
using System.Web.Mvc;

namespace Blog.Web.Actions.DisplayErrors
{
    public class DisplayErrorsController : Controller
    {
        public ActionResult Http404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        //https://stackoverflow.com/a/5507125/214073
        //https://stackoverflow.com/a/7499406/214073
        public ActionResult ThrowError()
        {
            throw new NotImplementedException("Test Error");
        }
    }
}