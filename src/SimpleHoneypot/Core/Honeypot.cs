// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Honeypot.cs" company="Web Advanced">
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
    using System.Web;
    using System.Web.Mvc;

    using SimpleHoneypot.Core.Common;

    public static class Honeypot {
        #region Constants and Fields

        public const string HttpContextKey = "__hpIsBot";

        public static readonly HoneypotWorker Worker;

        #endregion

        #region Constructors and Destructors

        static Honeypot() {
            SetDefaults();
            InputNames = new HoneypotInputNameCollection();
            Worker = new HoneypotWorker();
            CustomRules = new HoneypotRuleCollection();
        }

        #endregion

        #region Public Properties

        public static string CssClassName { get; private set; }

        public static string DefaultInputName { get; private set; }

        public static HoneypotInputNameCollection InputNames { get; private set; }

        public static HoneypotRuleCollection CustomRules { get; private set; }

        public static bool ManuallyHandleBots { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static MvcHtmlString GetHtml(HtmlHelper helper) {
            return Worker.GetHtml(helper, new HttpContextWrapper(HttpContext.Current));
        }

        public static bool IsBot(HttpContextBase context) {
            return Worker.IsBot(context);
        }

        public static void SeDefaultInputName(string inputName) {
            Check.Argument.IsNotNullOrEmpty(inputName, "inputName");
            DefaultInputName = inputName;
        }

        public static void SetCssClassName(string cssClassName) {
            Check.Argument.IsNotNullOrEmpty(cssClassName, "cssClassName");
            CssClassName = cssClassName;
        }

        public static void SetDefaults() {
            CssClassName = HoneypotData.DefaultCssClassName;
            DefaultInputName = HoneypotData.DefaultFieldName;
            ManuallyHandleBots = false;
        }

        public static void SetManuallyHandleBots(bool handleBots) {
            ManuallyHandleBots = handleBots;
        }

        #endregion
    }
}