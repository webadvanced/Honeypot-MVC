using System.Collections.Specialized;
using SimpleHoneypot.Core.Common.Thermador.Core.Common;

namespace SimpleHoneypot.Core.Services {
    public class HoneypotService : IHoneypotService {
        #region IHoneypotService Members

        public bool IsBot(NameValueCollection form, string key) {
            Check.Argument.IsNotNull(form, "form");
            Check.Argument.IsNotNullOrEmpty(key, "key");
            string val = form[key] ?? string.Empty;
            return !string.IsNullOrEmpty(val);
        }

        #endregion
    }
}