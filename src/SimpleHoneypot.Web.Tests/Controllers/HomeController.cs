using System.Web.Mvc;
using SimpleHoneypot.Web.Tests.Models;
using SimpleHoneypot.ActionFilters;

namespace SimpleHoneypot.Web.Tests.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About() {
            return View();
        }
        [Honeypot("/Home/About")]
        public ActionResult Subscribe(EmailSubscriber subscriber) {
            return RedirectToAction("Index");            
        }

    }
}
