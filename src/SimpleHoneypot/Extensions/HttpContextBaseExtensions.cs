using System;
using SimpleHoneypot.Core;
using System.Web;
using SimpleHoneypot.Core.Common.Thermador.Core.Common;

namespace SimpleHoneypot.Extensions {
    public static class HttpContextBaseExtensions {

        public static bool ValidateHoneypot(this HttpContextBase context) {
            Check.Argument.IsNotNull(context, "context");
            if (context.Items[Honeypot.HttpContextKey] == null)
                throw new NullReferenceException("HttpContext.Items must have entry for Honeypot.HttpContextKey. Ensure your action has HoneypotAttribute");
            return  (bool)context.Items[Honeypot.HttpContextKey];
            
        }
    }
}