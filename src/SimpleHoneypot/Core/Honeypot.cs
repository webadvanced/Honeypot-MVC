using System;

namespace SimpleHoneypot.Core {
    public static class Honeypot {
        public static readonly string TempDataKey = "Honeypot:Key";

        static Honeypot() {
            InputNames = new HoneypotInputNameCollection();
            CssClassName = "input-imp-long";
        }

        public static HoneypotInputNameCollection InputNames { get; private set; }
        public static string CssClassName { get; private set; }
        public static void SetCssClassName(string cssClassName) {
            if (string.IsNullOrEmpty(cssClassName))
                throw new ArgumentException("cssClassName cannot be null or empty", "cssClassName");
            CssClassName = cssClassName;
        }
    }
}