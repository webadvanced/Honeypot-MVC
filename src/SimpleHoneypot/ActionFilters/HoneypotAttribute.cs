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
    using System.Web.Mvc;

    using SimpleHoneypot.Core;
    using SimpleHoneypot.Core.Common;
    using SimpleHoneypot.Core.
    Services;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HoneypotAttribute : FilterAttribute, IAuthorizationFilter {
        #region Constants and Fields

        private readonly string redirectUrl;

        #endregion

        #region Constructors and Destructors

        public HoneypotAttribute() {
            this.redirectUrl = "/";
        }

        public HoneypotAttribute(bool manuallyHandleBots) {
            this.redirectUrl = "/";
        }

        public HoneypotAttribute(string redirectUrl) {
            Check.Argument.IsNotNullOrEmpty(redirectUrl, "redirectUrl");
            this.redirectUrl = redirectUrl;
        }

        #endregion

        #region Public Methods and Operators

        public void OnAuthorization(AuthorizationContext filterContext) {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            bool isBot = Honeypot.IsBot();

            if(!isBot || Honeypot.ManuallyHandleBots) {
                return;
            }

            filterContext.Result = new RedirectResult(this.redirectUrl);
        }

        #endregion
    }
}