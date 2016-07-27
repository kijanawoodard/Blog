using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

using static System.Configuration.ConfigurationManager;

namespace Blog.Web.Actions.Contact
{
    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PostModel model)
        {
            var isbot = !string.IsNullOrWhiteSpace(model.Access);
            var access = isbot ? $"BOT Access: {model.Access}\n\n" : "";
            var message = new MailMessage("contact-form@kijanawoodard.com", "contact-form@kijanawoodard.com");
            message.Subject = $"[Blog Contact] from {model.Name}";
            message.Body = access + model.Text;
            message.ReplyToList.Add(model.Email);

            var smtp = new SmtpClient(AppSettings["smtp::host"], int.Parse(AppSettings["smtp::port"]));
            smtp.Credentials = new NetworkCredential(AppSettings["smtp::username"], AppSettings["smtp::password"]);
            smtp.EnableSsl = bool.Parse(AppSettings["smtp::usessl"]);
            
            smtp.Send(message);
            
            return RedirectToAction("Thanks", "Contact");
        }

        [HttpGet]
        public ActionResult Thanks()
        {
            return View();
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
