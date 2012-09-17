// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotHelper.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleHoneypot.HtmlHelpers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using SimpleHoneypot.Core;

    public static class HoneypotHelper {
        #region Constants and Fields

        private static readonly Random Random = new Random();

        #endregion

        #region Public Methods and Operators

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
            return helper.TextBox(inputName, string.Empty, new { @class = Honeypot.CssClassName });
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

        #endregion
    }
}