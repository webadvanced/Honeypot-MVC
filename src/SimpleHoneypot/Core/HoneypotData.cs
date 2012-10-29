// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotData.cs" company="Web Advanced">
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
    using System;

    using SimpleHoneypot.Core.Common;

    public class HoneypotData {
        private string inputNameValue;

        #region Constants and Fields

        public const string DefaultCssClassName = "input-imp-long";

        public const string DefaultFieldName = "Phone-Data-Home";

        public const string FormKeyFieldName = "__hpKey";

        #endregion

        #region Constructors and Destructors

        public HoneypotData() {
        }

        public HoneypotData(string inputNameValue) {
            this.InputNameValue = inputNameValue;
            this.CreationDate = DateTime.UtcNow;
            this.Key = Guid.NewGuid();
        }

        #endregion

        #region Public Properties

        public DateTime CreationDate { get; set; }

        public string InputNameValue {
            get {
                return this.inputNameValue;
            }
            set {
                Check.Argument.IsNotNullOrEmpty(value, "InputNameValue");
                this.inputNameValue = value;
            }
        }

        public Guid Key { get; set; }

        #endregion

        #region Public Methods and Operators

        public static HoneypotData Create(string inputNameValue) {
            return new HoneypotData(inputNameValue);
        }

        #endregion
    }
}