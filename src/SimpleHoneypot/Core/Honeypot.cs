using SimpleHoneypot.Core.Common.Thermador.Core.Common;

namespace SimpleHoneypot.Core {
    public static class Honeypot {
        public static readonly string TempDataKey = "Honeypot:Key";
        public static readonly string HttpContextKey = "Honeypot:IsBot";
        static Honeypot() {
            InputNames = new HoneypotInputNameCollection();
            CssClassName = "input-imp-long";
            DefaultInputName = "Phone-Data-Home";
            ManuallyHandleBots = false;
        }

        public static HoneypotInputNameCollection InputNames { get; private set; }
        public static string CssClassName { get; private set; }
        public static string DefaultInputName { get; private set; }
        public static bool ManuallyHandleBots { get; private set; }

        public static void SetCssClassName(string cssClassName) {
            Check.Argument.IsNotNullOrEmpty(cssClassName, "cssClassName");
            CssClassName = cssClassName;
        }

        public static void SeDefaultInputName(string inputName) {
            Check.Argument.IsNotNullOrEmpty(inputName, "inputName");
            DefaultInputName = inputName;
        }

        public static void SetManuallyHandleBots(bool b) {
            ManuallyHandleBots = b;
        }
    }
}