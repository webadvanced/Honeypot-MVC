using System.Collections.Specialized;
using Xunit;
using SimpleHoneypot.Core.Services;
using System;

namespace SimpleHoneypot.Tests {
    public class HoneypotServiceTests {
        public const string MockKey = "MockKey";
        [Fact]
        public void IsBot_ShouldThrowArgumentNullException_WhenPassedNullForForm() {
            var honeypotService = new HoneypotService();
            Assert.Throws<ArgumentNullException>(() => honeypotService.IsBot(null, "Valid Key"));
        }

        [Fact]
        public void IsBot_ShouldThrowArgumentException_WhenPassedNullOrEmptyForKey() {
            var honeypotService = new HoneypotService();
            Assert.Throws<ArgumentException>(() => honeypotService.IsBot(new NameValueCollection(), String.Empty));
            Assert.Throws<ArgumentException>(() => honeypotService.IsBot(new NameValueCollection(), null));
        }

        [Fact]
        public void IsBot_ShouldBeFalseWhen_FormValueIsEmpyString() {
            var honeypotService = new HoneypotService();
            var result = honeypotService.IsBot(new NameValueCollection {{MockKey, ""}}, MockKey);
            Assert.False(result);
        }

        [Fact]
        public void IsBot_ShouldBeTrueWhen_FormValueIsNotEmpyString() {
            var honeypotService = new HoneypotService();
            var result = honeypotService.IsBot(new NameValueCollection { { MockKey, "Fail" } }, MockKey);
            Assert.True(result);
        }
    }
}