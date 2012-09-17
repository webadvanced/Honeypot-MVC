using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SimpleHoneypot.Core;
using System.Collections.Generic;

namespace SimpleHoneypot.HtmlHelpers {
    public static class HoneypotHelper {

        private static readonly Random Random = new Random();

        public static MvcHtmlString HoneypotInput(this HtmlHelper helper) {
            string inputName;
            if (Honeypot.InputNames.Count < 2) {
                inputName = Honeypot.DefaultInputName;
            }
            else {
                
                string[] keys = Honeypot.InputNames.Shuffle().Take(2).ToArray();
                inputName = String.Format("{0}-{1}", keys[0], keys[1]);
            }
            helper.ViewContext.Controller.TempData[Honeypot.TempDataKey] = inputName;
            return helper.TextBox(inputName, string.Empty, new {@class = Honeypot.CssClassName});
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i > 0; i--) {
                int swapIndex = Random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
            yield return elements[0];
        }
    }
}