using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.HtmlHelpers {
    public static class HoneypotHelper {
        

        public static MvcHtmlString HoneypotInput(this HtmlHelper helper) {
            string inputName;
            if (Honeypot.InputNames.Count < 2) {
                inputName = Honeypot.DefaultInputName;
            }
            else {
                var random = new Random(Honeypot.InputNames.Count);
                string[] keys = Honeypot.InputNames.OrderBy(x => random.Next()).Take(2).ToArray();
                inputName = String.Format("{0}-{1}", keys[0], keys[1]);
            }
            helper.ViewContext.TempData[Honeypot.TempDataKey] = inputName;
            return helper.TextBox(inputName, string.Empty, new {@class = Honeypot.CssClassName});
        }
    }
}