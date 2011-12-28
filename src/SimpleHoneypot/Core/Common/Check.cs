namespace SimpleHoneypot.Core.Common {
    using System;
    using System.Diagnostics;

    namespace Thermador.Core.Common {
        public static class Check {
            public static class Argument {
                [DebuggerStepThrough]
                public static void IsNotNull(object parameter, string parameterName) {
                    if (parameter == null) {
                        throw new ArgumentNullException(parameterName, String.Format(GlobalText.ArgumentCannotBeNull, parameterName));
                    }
                }

                [DebuggerStepThrough]
                public static void IsNotNullOrEmpty(string parameter, string parameterName) {
                    if (string.IsNullOrWhiteSpace(parameter)) {
                        throw new ArgumentException(String.Format(GlobalText.ArgumentCannotBeNullOrEmpty, parameterName), parameterName);
                    }
                }

            }

        }
    }

}