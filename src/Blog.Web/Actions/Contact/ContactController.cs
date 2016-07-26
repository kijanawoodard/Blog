using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

using static System.Configuration.ConfigurationManager;

namespace Blog.Web.Actions.Contact
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PostModel model)
        {
            var smtp = new SmtpClient(AppSettings["smtp::host"], int.Parse(AppSettings["smtp::port"]));
            smtp.Credentials = new NetworkCredential(AppSettings["smtp::username"], AppSettings["smtp::password"]);
            smtp.EnableSsl = bool.Parse(AppSettings["smtp::usessl"]);

            smtp.Send("contact-form@kijanawoodard.com", "contact-form@kijanawoodard.com", $"[Blog Contact] from {model.Name}", model.Email + "\n\nAccess:" + model.Access + "\n\n" + model.Text);

            
            return RedirectToAction("Index", "Contact");
        }

        public class PostModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Text { get; set; }
            public string Access { get; set; }
        }
    }
}
