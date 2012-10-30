namespace SimpleHoneypot.Tests {
    using System;

    using SimpleHoneypot.Core;

    using Xunit;

    public class HoneypotDataTests {
        #region Public Methods and Operators

        [Fact]
        public void Ctor_ShouldSetInputNameValue_WhenPassedValidString() {
            var honeypotData = new HoneypotData("Valid String");
            Assert.Equal(honeypotData.InputNameValue, "Valid String");
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentException_WhenPassedNullOrEmpty() {
            Assert.Throws<ArgumentException>(() => new HoneypotData(null));
            Assert.Throws<ArgumentException>(() => new HoneypotData(string.Empty));
        }

        #endregion
    }
}