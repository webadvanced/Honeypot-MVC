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
            AuthorizationContext filterContext = new Mock<AuthorizationContext> {DefaultValue = DefaultValue.Mock}.Object;
            var attribue = new HoneypotAttribute();

            Assert.Throws<InvalidOperationException>(() => attribue.OnAuthorization(filterContext));
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenHoneypotKeyIsNotInTempData() {
            var filterContext = MvcHelper.BuildAuthorizationContext(false);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToRootByDefault_WhenTempDataKeyHasValue() {
            var filterContext = MvcHelper.BuildAuthorizationContext(true);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult) filterContext.Result;
            Assert.Equal("/", result.Url);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToProvidedUrl_WhenTempDataKeyHasValue() {
            var filterContext = MvcHelper.BuildAuthorizationContext(true);
            var attribue = new HoneypotAttribute("/Handle/Bot");

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult)filterContext.Result;
            Assert.Equal("/Handle/Bot", result.Url);
        }
    }
}