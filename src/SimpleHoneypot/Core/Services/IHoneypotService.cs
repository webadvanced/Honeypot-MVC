using System.Collections.Specialized;

namespace SimpleHoneypot.Core.Services {
    public interface IHoneypotService {
        bool IsBot(NameValueCollection form, string key);
    }
}