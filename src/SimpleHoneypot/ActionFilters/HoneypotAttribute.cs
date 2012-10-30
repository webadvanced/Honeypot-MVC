// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotAttribute.cs" company="Web Advanced">
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

namespace SimpleHoneypot.ActionFilters {
    using System;
    using System.Web;
    using System.Web.Mvc;

    using SimpleHoneypot.Core;
    using SimpleHoneypot.Core.Common;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HoneypotAttribute : FilterAttribute, IAuthorizationFilter {
        #region Constants and Fields

        private readonly bool manuallyHandleBots;

        private readonly string redirectUrl;

        private Func<HttpContextBase, bool> isBotFunc;

        #endregion

        #region Constructors and Destructors

        public HoneypotAttribute(bool manuallyHandleBots = false, string redirectUrl = "/")
            : this(Honeypot.IsBot, manuallyHandleBots, redirectUrl) {
        }

        internal HoneypotAttribute(
            Func<HttpContextBase, bool> isBotAction, bool manuallyHandleBots = false, string redirectUrl = "/") {
            this.IsBotFunc = isBotAction;
            this.redirectUrl = redirectUrl;
            this.manuallyHandleBots = manuallyHandleBots;
        }

        #endregion

        #region Properties

        internal Func<HttpContextBase, bool> IsBotFunc {
            get {
                return this.isBotFunc;
            }
            private set {
                Check.Argument.IsNotNull(value, "IsBotFunc");
                this.isBotFunc = value;
            }
        }

        #endregion

        //For Tests

        #region Public Methods and Operators

        public static HoneypotAttribute CreateForTests(Func<HttpContextBase, bool> isBotAction, bool manuallyHandleBots = false, string redirectUrl = "/") {
            return new HoneypotAttribute(isBotAction, manuallyHandleBots, redirectUrl);
        }

        public void OnAuthorization(AuthorizationContext filterContext) {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            bool isBot = this.IsBotFunc(filterContext.HttpContext);

            if (!isBot || this.manuallyHandleBots || Honeypot.ManuallyHandleBots) {
                return;
            }

            filterContext.Result = new RedirectResult(this.redirectUrl);
        }

        #endregion
    }
}