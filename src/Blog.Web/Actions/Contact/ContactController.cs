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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(PostModel model)
        {
            var isbot = !string.IsNullOrWhiteSpace(model.Message) || !string.IsNullOrWhiteSpace(model.Instructions) || model.Confirm;
            var botSubject = isbot ? "[BOT] " : "";
            var botBody = isbot ? $"BOT Message:{model.Message}\nInstructions: {model.Instructions}\nBot confirm: {model.Confirm}\n\n" : "";
            var message = new MailMessage("contact-form@kijanawoodard.com", "contact-form@kijanawoodard.com");
            message.Subject = $"{botSubject}[Blog Contact] from {model.Name}";
            message.Body = botBody + model.Text;

            if (!string.IsNullOrWhiteSpace(model.Email))
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
            public string Message { get; set; }
            public string Instructions { get; set; }
            public bool Confirm { get; set; }
        }
    }
}
