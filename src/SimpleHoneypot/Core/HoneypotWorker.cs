// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotWorker.cs" company="Web Advanced">
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

namespace SimpleHoneypot.Core {
    using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

    public class HoneypotWorker {
        #region Constants and Fields

        private static readonly Random Random = new Random();

        #endregion

        #region Constructors and Destructors

        public HoneypotWorker() {
            this.Serializer = new HoneypotDataSerializer();
        }

        #endregion

        #region Properties

        internal HoneypotDataSerializer Serializer { get; private set; }

        #endregion

        #region Public Methods and Operators

        public MvcHtmlString GetHtml(HtmlHelper helper, HttpContextBase httpContext) {
            HoneypotData token = this.GetHoneypotData(httpContext);
            String hpInput = helper.TextBox(token.InputNameValue, string.Empty, new { @class = Honeypot.CssClassName }).ToString();
            String hpKey = helper.Hidden(HoneypotData.FormKeyFieldName, Serializer.Serialize(token)).ToString();
            return MvcHtmlString.Create(hpInput + hpKey);
        }

        //true if valid
        public bool IsBot(HttpContextBase httpContext) {
            string tokenString = httpContext.Request.Form[HoneypotData.FormKeyFieldName];
            if (string.IsNullOrEmpty(tokenString)) {
                httpContext.Items.Add(Honeypot.HttpContextKey, true);
                return true;
            }

            if(!this.ValidateRules(httpContext.Request.Form)) {
                return true;
            }

            HoneypotData token = this.Serializer.Deserialize(tokenString);

            string trap = httpContext.Request.Form[token.InputNameValue];

            bool isBot = (trap == null) || (trap.Length > 0);
            httpContext.Items.Add(Honeypot.HttpContextKey, isBot);
            return isBot;
        }

        #endregion

        #region Methods

        public bool ValidateRules(NameValueCollection form) {
            if(!Honeypot.CustomRules.Any()) {
                return true;
            }

            foreach(var rule in Honeypot.CustomRules) {
                bool result = rule(form);
                if (result) return false;
            }

            return true;
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source) {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i > 0; i--) {
                int swapIndex = Random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
            yield return elements[0];
        }

        public HoneypotData GetHoneypotData(HttpContextBase httpContext) {
            string inputName;
            if (Honeypot.InputNames.Count < 2) {
                inputName = Honeypot.DefaultInputName;
            }
            else {
                string[] keys = Shuffle(Honeypot.InputNames).Take(2).ToArray();
                inputName = String.Format("{0}-{1}", keys[0], keys[1]);
            }

            return HoneypotData.Create(inputName);   
        }

        #endregion
    }
}