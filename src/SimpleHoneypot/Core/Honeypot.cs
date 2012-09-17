// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Honeypot.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleHoneypot.Core {
    using SimpleHoneypot.Core.Common.Thermador.Core.Common;

    public static class Honeypot {
        #region Constants and Fields

        public static readonly string HttpContextKey = "Honeypot:IsBot";

        public static readonly string TempDataKey = "Honeypot:Key";

        #endregion

        #region Constructors and Destructors

        static Honeypot() {
            InputNames = new HoneypotInputNameCollection();
            CssClassName = "input-imp-long";
            DefaultInputName = "Phone-Data-Home";
            ManuallyHandleBots = false;
        }

        #endregion

        #region Public Properties

        public static string CssClassName { get; private set; }

        public static string DefaultInputName { get; private set; }

        public static HoneypotInputNameCollection InputNames { get; private set; }

        public static bool ManuallyHandleBots { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void SeDefaultInputName(string inputName) {
            Check.Argument.IsNotNullOrEmpty(inputName, "inputName");
            DefaultInputName = inputName;
        }

        public static void SetCssClassName(string cssClassName) {
            Check.Argument.IsNotNullOrEmpty(cssClassName, "cssClassName");
            CssClassName = cssClassName;
        }

        public static void SetManuallyHandleBots(bool b) {
            ManuallyHandleBots = b;
        }

        #endregion
    }
}