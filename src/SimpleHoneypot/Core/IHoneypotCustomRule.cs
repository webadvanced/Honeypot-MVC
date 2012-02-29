using System;

namespace SimpleHoneypot.Core {
    public interface IHoneypotCustomRule<in T> where T : class {
        bool IsValid(T obj);
    }
}