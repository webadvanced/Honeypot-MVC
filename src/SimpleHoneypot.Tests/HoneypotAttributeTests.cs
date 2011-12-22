using System;
using System.Web.Mvc;
using Moq;
using SimpleHoneypot.ActionFilters;
using Xunit;

namespace SimpleHoneypot.Tests {
    public class HoneypotAttributeTests {
        [Fact]
        public void OnAuthorization_ShouldThrowArgumentNullException_WhenFilterContextIsNull() {
            var attribue = new HoneypotAttribute();

            Assert.Throws<ArgumentNullException>(() => attribue.OnAuthorization(null));
        }

        [Fact]
        public void OnAuthorization_ShouldThrowInvalidOperactoinException_WhenTempDataDoesNotContainHoneypotKey() {
            AuthorizationContext filterContext =
                new Mock<AuthorizationContext> {DefaultValue = DefaultValue.Mock}.Object;
            var attribue = new HoneypotAttribute();

            Assert.Throws<InvalidOperationException>(() => attribue.OnAuthorization(filterContext));
        }
    }
}