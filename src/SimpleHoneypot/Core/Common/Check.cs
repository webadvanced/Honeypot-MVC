// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Check.cs" company="Web Advanced">
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

namespace SimpleHoneypot.Core.Common {
    namespace Thermador.Core.Common {
        using System;
        using System.Diagnostics;

        public static class Check {
            public static class Argument {
                #region Public Methods and Operators

                [DebuggerStepThrough]
                public static void IsNotNull(object parameter, string parameterName) {
                    if (parameter == null) {
                        throw new ArgumentNullException(
                            parameterName, String.Format(GlobalText.ArgumentCannotBeNull, parameterName));
                    }
                }

                [DebuggerStepThrough]
                public static void IsNotNullOrEmpty(string parameter, string parameterName) {
                    if (string.IsNullOrEmpty(parameter)) {
                        throw new ArgumentException(
                            String.Format(GlobalText.ArgumentCannotBeNullOrEmpty, parameterName), parameterName);
                    }
                }

                #endregion
            }
        }
    }
}