using System;
using System.Web.Mvc;
using Moq;
using SimpleHoneypot.ActionFilters;
using Xunit;
using System.Collections.Specialized;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Tests {
    public class HoneypotAttributeTests {

        private NameValueCollection _from = new NameValueCollection { { MvcHelper.FakeInputName, "I Am a Bot" } };

        [Fact]
        public void OnAuthorization_ShouldThrowArgumentNullException_WhenFilterContextIsNull() {
            var attribue = new HoneypotAttribute();

            Assert.Throws<ArgumentNullException>(() => attribue.OnAuthorization(null));
        }

        [Fact]
        public void OnAuthorization_isBotShoultBeTrue_WhenTempDataDoesNotContainHoneypotKey() {
            AuthorizationContext filterContext = new Mock<AuthorizationContext> {DefaultValue = DefaultValue.Mock}.Object;
            Honeypot.SetManuallyHandleBots(false);
            var attribue = new HoneypotAttribute(redirectUrl:"/Handle/Bot");

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult)filterContext.Result;
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenHoneypotKeyIsNotInTempData() {
            Honeypot.SetManuallyHandleBots(false);
            var filterContext = MvcHelper.GetAuthorizationContext();
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToRootByDefault_WhenTempDataKeyHasValue() {
            Honeypot.SetManuallyHandleBots(false);
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult) filterContext.Result;
            Assert.Equal("/", result.Url);
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenTempDataKeyHasValueAndManuallyHandleBotsIsTrueGlobally() {
            Honeypot.SetManuallyHandleBots(true);
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenTempDataKeyHasValueAndManuallyHandleBotsIsTrueOnAttribute() {
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            var attribue = new HoneypotAttribute(true);

            attribue.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToProvidedUrl_WhenTempDataKeyHasValue() {
            Honeypot.SetManuallyHandleBots(false);
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            var attribue = new HoneypotAttribute(redirectUrl:"/Handle/Bot");

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult)filterContext.Result;
            Assert.Equal("/Handle/Bot", result.Url);
        }
    }
}