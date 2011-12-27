using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using SimpleHoneypot.ActionFilters;
using SimpleHoneypot.Core;
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
            var filterContext = BuildAuthorizationContext(false);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToRootByDefault_WhenTempDataKeyHasValue() {
            var filterContext = BuildAuthorizationContext(true);
            var attribue = new HoneypotAttribute();

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult) filterContext.Result;
            Assert.Equal("/", result.Url);
        }

        [Fact]
        public void OnAuthorization_ShouldRedirectToProvidedUrl_WhenTempDataKeyHasValue() {
            var filterContext = BuildAuthorizationContext(true);
            var attribue = new HoneypotAttribute("/Handle/Bot");

            attribue.OnAuthorization(filterContext);

            Assert.Equal("RedirectResult", filterContext.Result.GetType().Name);
            var result = (RedirectResult)filterContext.Result;
            Assert.Equal("/Handle/Bot", result.Url);
        }

        public AuthorizationContext BuildAuthorizationContext(bool addFormValue) {
            string fakeInputName = "Fake-Input-Name";
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("POST");
            request.Setup(r => r.Headers).Returns(new NameValueCollection());
            request.Setup(r => r.Form).Returns(new NameValueCollection());
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());
            request.Setup(r => r.Files).Returns(new Mock<HttpFileCollectionBase>().Object);
            
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(request.Object);
            mockHttpContext.Setup(c => c.Session).Returns((HttpSessionStateBase) null);
            if(addFormValue) {
                var form = new NameValueCollection {{fakeInputName, "I Am A Spam Bot!"}};
                mockHttpContext.Setup(c => c.Request.Form).Returns(form);
            }
            
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            controllerContext.Controller.TempData = new TempDataDictionary();
            controllerContext.Controller.TempData.Add(Honeypot.TempDataKey, "Fake-Input-Name");
            

            return new AuthorizationContext(controllerContext);
        }
    }
}