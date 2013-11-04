using System.Web.Mvc;

namespace Blog.Web.Actions.DisplayErrors
{
    public class DisplayErrorsController : Controller
    {
        public ActionResult Http404()
        {
            return View();
        }

    }
}