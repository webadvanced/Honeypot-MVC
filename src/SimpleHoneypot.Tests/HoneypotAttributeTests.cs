using System;
using System.Web.Mvc;
using Moq;
using SimpleHoneypot.ActionFilters;
using Xunit;
using System.Collections.Specialized;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Tests {
    using System.Web;

    public class HoneypotAttributeTests {

        private NameValueCollection _from = new NameValueCollection { { MvcHelper.FakeInputName, "I Am a Bot" } };

        [Fact]
        public void OnAuthorization_ShouldThrowArgumentNullException_WhenFilterContextIsNull() {
            var attribue = new HoneypotAttribute();

            Assert.Throws<ArgumentNullException>(() => attribue.OnAuthorization(null));
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
        public void CreateForTests_ShouldThrowArgumentNullException_WhenPassedNull() {
            Assert.Throws<ArgumentNullException>(() => HoneypotAttribute.CreateForTests(null));
        }

        [Fact]
        public void OnAuthorization_ForwardsAttributes()
        {
            // Arrange
            HttpContextBase context = new Mock<HttpContextBase>().Object;
            Mock<AuthorizationContext> authorizationContextMock = new Mock<AuthorizationContext>();
            authorizationContextMock.SetupGet(ac => ac.HttpContext).Returns(context);
            bool isBotCalled = false;
            Func<HttpContextBase, bool> isBotMethod = (c) => {
                Assert.Same(context, c);
                isBotCalled = true;
                return false;
            };

            HoneypotAttribute attribute = HoneypotAttribute.CreateForTests(isBotMethod);
            

            // Act
            attribute.OnAuthorization(authorizationContextMock.Object);

            // Assert
            Assert.True(isBotCalled);
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenIsBotFuncReturnsFalse() {
            // Arrange
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            Honeypot.SetManuallyHandleBots(false);
            Func<HttpContextBase, bool> isBotMethod = (c) => false;
            var attribue = HoneypotAttribute.CreateForTests(isBotMethod, manuallyHandleBots: false);

            // Act
            attribue.OnAuthorization(filterContext);

            // Assert
            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenIsBotFuncReturnsTrueAndManuallyHandleBotsIsTrueForInstance()
        {
            // Arrange
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            Honeypot.SetManuallyHandleBots(false);
            Func<HttpContextBase, bool> isBotMethod = (c) => true;
            var attribue = HoneypotAttribute.CreateForTests(isBotMethod, manuallyHandleBots: true);

            // Act
            attribue.OnAuthorization(filterContext);

            // Assert
            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldReturn_WhenIsBotFuncReturnsTrueAndGlobalManuallyHandleBotsIsTrue()
        {
            // Arrange
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            Honeypot.SetManuallyHandleBots(true);
            Func<HttpContextBase, bool> isBotMethod = (c) => true;
            var attribue = HoneypotAttribute.CreateForTests(isBotMethod, manuallyHandleBots: false);

            // Act
            attribue.OnAuthorization(filterContext);

            // Assert
            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToProvidedUrl() {
            // Arrange
            Honeypot.SetManuallyHandleBots(false);
            var filterContext = MvcHelper.GetAuthorizationContext(_from);
            Func<HttpContextBase, bool> isBotMethod = (c) => true;
            var attribue = HoneypotAttribute.CreateForTests(isBotMethod, redirectUrl: "/handle/bot");

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult)filterContext.Result;
            Assert.Equal("/handle/bot", result.Url);
        }
    }
}