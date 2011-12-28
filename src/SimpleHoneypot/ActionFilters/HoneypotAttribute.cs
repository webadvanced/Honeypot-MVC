using System;
using System.Web.Mvc;
using SimpleHoneypot.Core;
using SimpleHoneypot.Core.Common.Thermador.Core.Common;
using SimpleHoneypot.Core.Services;

namespace SimpleHoneypot.ActionFilters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HoneypotAttribute : FilterAttribute, IAuthorizationFilter {
        private readonly string _redirectUrl;
        private readonly IHoneypotService _honeypotService;
        public HoneypotAttribute() {
            _redirectUrl = "/";
            _honeypotService = new HoneypotService();
        }

        public HoneypotAttribute(string redirectUrl) {
            Check.Argument.IsNotNullOrEmpty(redirectUrl, "redirectUrl");
            _redirectUrl = redirectUrl;
            _honeypotService = new HoneypotService();
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext) {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            if (filterContext.Controller.TempData[Honeypot.TempDataKey] == null)
                throw new InvalidOperationException("HoneypotInput must be in the form collection");

            string key = filterContext.Controller.TempData[Honeypot.TempDataKey].ToString();
            bool isBot = _honeypotService.IsBot(filterContext.HttpContext.Request.Form, key);
            filterContext.HttpContext.Items.Add(Honeypot.HttpContextKey, isBot);
            if (!isBot || !Honeypot.AutomaticallyHandleBots) return;
            
            filterContext.Result = new RedirectResult(_redirectUrl);
        }

        #endregion
    }
}