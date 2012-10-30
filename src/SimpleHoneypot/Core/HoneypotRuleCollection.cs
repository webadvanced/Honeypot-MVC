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