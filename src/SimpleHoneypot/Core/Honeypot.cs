using System;
using System.Web;

namespace SimpleHoneypot.Core {
    public static class Honeypot {
        public static readonly string TempDataKey = "Honeypot:Key";
        static Honeypot() {
            InputNames = new HoneypotInputNameCollection();
            CssClassName = "input-imp-long";
            DefaultInputName = "Phone-Data-Home";
            AutomaticallyHandleBots = true;
        }

        public static HoneypotInputNameCollection InputNames { get; private set; }
        public static string CssClassName { get; private set; }
        public static string DefaultInputName { get; private set; }
        public static bool AutomaticallyHandleBots { get; private set; }

        public static void SetCssClassName(string cssClassName) {
            if (string.IsNullOrEmpty(cssClassName))
                throw new ArgumentException("cssClassName cannot be null or empty", "cssClassName");
            CssClassName = cssClassName;
        }

        public static void SeDefaultInputName(string inputName) {
            if (string.IsNullOrEmpty(inputName))
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            DefaultInputName = inputName;
        }

        public static void SetAutomaticallyHandleBots(bool b) {
            AutomaticallyHandleBots = b;
        }
    }
}