// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotInputNameCollection.cs" company="Web Advanced">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class HoneypotInputNameCollection : IEnumerable<string> {
        #region Constants and Fields

        private readonly List<string> inputNames = new List<string>();

        #endregion

        #region Public Properties

        public int Count {
            get {
                return this.inputNames.Count;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add(string inputName) {
            if (string.IsNullOrEmpty(inputName)) {
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            }
            this.inputNames.Add(inputName);
        }

        public void Add(IEnumerable<string> inputNames) {
            if (inputNames == null) {
                throw new ArgumentNullException("inputNames", "inputNames cannot be null");
            }

            foreach (string inputName in inputNames) {
                Add(inputName);
            }
        }

        public void Clear() {
            this.inputNames.Clear();
        }

        public bool Contains(string inputName) {
            if (string.IsNullOrEmpty(inputName)) {
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            }
            return this.inputNames.Any(x => x == inputName);
        }

        public IEnumerator<string> GetEnumerator() {
            return this.inputNames.GetEnumerator();
        }

        public void Remove(string inputName) {
            if (string.IsNullOrEmpty(inputName)) {
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            }
            this.inputNames.Remove(inputName);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator() {
            return this.inputNames.GetEnumerator();
        }

        #endregion
    }
}