using System.Web.Mvc;
using SimpleHoneypot.Extensions;
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

        [Honeypot(true)]
        public ActionResult SubscribeManual(EmailSubscriber subscriber) {
            //Handle request if the Honeypot Faild
            if (Request.HoneypotFaild())
                return RedirectToAction("About");
            
            return RedirectToAction("Index");
        }

    }
}
