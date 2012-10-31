// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HoneypotRuleCollection.cs" company="Web Advanced">
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
    using System.Collections.Specialized;

    using SimpleHoneypot.Core.Common;

    public class HoneypotRuleCollection : IEnumerable<Func<NameValueCollection, bool>> {
        #region Constants and Fields

        private readonly List<Func<NameValueCollection, bool>> rules = new List<Func<NameValueCollection, bool>>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// A func that returns true if request is a bot
        /// </summary>
        /// <param name="func"></param>
        public void Add(Func<NameValueCollection, bool> func) {
            Check.Argument.IsNotNull(func, "func");
            this.rules.Add(func);
        }

        public void Clear() {
            this.rules.Clear();
        }

        public IEnumerator<Func<NameValueCollection, bool>> GetEnumerator() {
            return this.rules.GetEnumerator();
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion
    }
}