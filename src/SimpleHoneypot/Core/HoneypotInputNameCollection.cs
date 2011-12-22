using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHoneypot.Core {
    public sealed class HoneypotInputNameCollection : IEnumerable<string> {
        private readonly List<string> _inputNames = new List<string>();

        public int Count {
            get { return _inputNames.Count; }
        }

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator() {
            return _inputNames.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _inputNames.GetEnumerator();
        }

        #endregion

        public void Add(string inputName) {
            if (string.IsNullOrEmpty(inputName))
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            _inputNames.Add(inputName);
        }

        public void Add(IEnumerable<string> inputNames) {
            if (inputNames == null)
                throw new ArgumentNullException("inputNames", "inputNames cannot be null");

            foreach (string inputName in inputNames) {
                Add(inputName);
            }
        }

        public void Remove(string inputName) {
            if (string.IsNullOrEmpty(inputName))
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            _inputNames.Remove(inputName);
        }

        public void Clear() {
            _inputNames.Clear();
        }

        public bool Contains(string inputName) {
            if (string.IsNullOrEmpty(inputName))
                throw new ArgumentException("inputName cannot be null or empty", "inputName");
            return _inputNames.Any(x => x == inputName);
        }
    }
}