using System;
using System.Web.Mvc;

namespace Blog.Web.Actions.DisplayErrors
{
    public class DisplayErrorsController : Controller
    {
        public ActionResult Http404()
        {
            return View();
        }

        //http://stackoverflow.com/a/5507125/214073
        //http://stackoverflow.com/a/7499406/214073
        public ActionResult ThrowError()
        {
            throw new NotImplementedException("Test Error");
        }
    }
}