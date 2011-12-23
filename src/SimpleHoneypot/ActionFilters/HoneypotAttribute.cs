using System;
using System.Web.Mvc;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.ActionFilters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HoneypotAttribute : FilterAttribute, IAuthorizationFilter {
        private readonly string _redirectUrl;

        public HoneypotAttribute() {
            _redirectUrl = "/";
        }

        public HoneypotAttribute(string redirectUrl) {
            _redirectUrl = redirectUrl;
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext", "filterContext cannot be null");

            if (filterContext.Controller.TempData[Honeypot.TempDataKey] == null)
                throw new InvalidOperationException("HoneypotInput must be in the executing form collection");
            string val =
                filterContext.HttpContext.Request.Form[
                    filterContext.Controller.TempData[Honeypot.TempDataKey].ToString()];
            if (string.IsNullOrWhiteSpace(val)) return;

            HandelFailedRequest(filterContext);
        }

        #endregion

        protected virtual void HandelFailedRequest(AuthorizationContext filterContext) {
            //Redirect to the root returning a HTTP 200
            filterContext.HttpContext.Response.Redirect(_redirectUrl);
        }
    }
}