using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Moq;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Tests {
    public static class MvcHelper {
        public const string AppPathModifier = "/$(SESSION)";
        public const string FakeInputName = "Fake-Input-Name";
        public static HtmlHelper<object> GetHtmlHelper() {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null);
            var rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null)
                   {Defaults = new RouteValueDictionary(new {id = "defaultid"})});
            rt.Add("namedroute",
                   new Route("named/{controller}/{action}/{id}", null)
                   {Defaults = new RouteValueDictionary(new {id = "defaultid"})});
            var rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            var vdd = new ViewDataDictionary();

            var viewContext = new ViewContext
                              {
                                  HttpContext = httpcontext,
                                  RouteData = rd,
                                  ViewData = vdd,
                                  Controller = GetControllerContext().Controller
                              };
            var mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);
            
            var htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);
            return htmlHelper;
        }

        public static HtmlHelper GetHtmlHelper(string protocol, int port) {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null, protocol, port, null);
            var rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null)
                   {Defaults = new RouteValueDictionary(new {id = "defaultid"})});
            rt.Add("namedroute",
                   new Route("named/{controller}/{action}/{id}", null)
                   {Defaults = new RouteValueDictionary(new {id = "defaultid"})});
            var rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            var vdd = new ViewDataDictionary();

            var mockViewContext = new Mock<ViewContext>();
            mockViewContext.Setup(c => c.HttpContext).Returns(httpcontext);
            mockViewContext.Setup(c => c.RouteData).Returns(rd);
            mockViewContext.Setup(c => c.ViewData).Returns(vdd);
            var mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            var htmlHelper = new HtmlHelper(mockViewContext.Object, mockVdc.Object, rt);
            return htmlHelper;
        }

        public static HtmlHelper GetHtmlHelper(ViewDataDictionary viewData) {
            var mockViewContext = new Mock<ViewContext> {CallBase = true};
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper(mockViewContext.Object, container);
        }

        public static HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData) {
            var mockViewContext = new Mock<ViewContext> {CallBase = true};
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            IViewDataContainer container = GetViewDataContainer(viewData);
            return new HtmlHelper<TModel>(mockViewContext.Object, container);
        }

        public static HtmlHelper GetHtmlHelperWithPath(ViewDataDictionary viewData) {
            return GetHtmlHelperWithPath(viewData, "/");
        }

        public static HtmlHelper GetHtmlHelperWithPath(ViewDataDictionary viewData, string appPath) {
            ViewContext viewContext = GetViewContextWithPath(appPath, viewData);
            var mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            IViewDataContainer container = mockContainer.Object;
            return new HtmlHelper(viewContext, container, new RouteCollection());
        }

        public static HtmlHelper<TModel> GetHtmlHelperWithPath<TModel>(ViewDataDictionary<TModel> viewData,
                                                                       string appPath) {
            ViewContext viewContext = GetViewContextWithPath(appPath, viewData);
            var mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            IViewDataContainer container = mockContainer.Object;
            return new HtmlHelper<TModel>(viewContext, container, new RouteCollection());
        }

        public static HtmlHelper<TModel> GetHtmlHelperWithPath<TModel>(ViewDataDictionary<TModel> viewData) {
            return GetHtmlHelperWithPath(viewData, "/");
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod,
                                                     string protocol, int port, NameValueCollection form) {
            var mockHttpContext = new Mock<HttpContextBase>();

            if (!String.IsNullOrEmpty(appPath)) {
                mockHttpContext.Setup(o => o.Request.ApplicationPath).Returns(appPath);
            }
            if (!String.IsNullOrEmpty(requestPath)) {
                mockHttpContext.Setup(o => o.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri;

            if (port >= 0) {
                uri = new Uri(protocol + "://localhost" + ":" + Convert.ToString(port));
            }
            else {
                uri = new Uri(protocol + "://localhost");
            }
            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);

            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod)) {
                mockHttpContext.Setup(o => o.Request.HttpMethod).Returns(httpMethod);
            }
            if (form != null) {
                mockHttpContext.Setup(c => c.Request.Form).Returns(form);
            }
            else {
                mockHttpContext.Setup(c => c.Request.Form).Returns(new NameValueCollection());
            }
            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase) null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(
                r => AppPathModifier + r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp, -1, null);
        }

        public static HttpContextBase GetHttpContext(NameValueCollection form) {
            return GetHttpContext("", "/", "POST", Uri.UriSchemeHttp, -1, form);
        }

        public static HttpContextBase GetHttpContext() {
            return GetHttpContext("", "/", "POST", Uri.UriSchemeHttp, -1, null);
        }

        public static ViewContext GetViewContextWithPath(string appPath, ViewDataDictionary viewData) {
            HttpContextBase httpContext = GetHttpContext(appPath, "/request", "GET");

            var mockViewContext = new Mock<ViewContext> {DefaultValue = DefaultValue.Mock};
            mockViewContext.Setup(c => c.HttpContext).Returns(httpContext);
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.Writer).Returns(new StringWriter());
            return mockViewContext.Object;
        }

        public static ViewContext GetViewContextWithPath(ViewDataDictionary viewData) {
            return GetViewContextWithPath("/", viewData);
        }

        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData) {
            var mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }

        public static AuthorizationContext GetAuthorizationContext(NameValueCollection form) {
            return new AuthorizationContext(GetControllerContext(form));
        }
        public static AuthorizationContext GetAuthorizationContext() {
            return new AuthorizationContext(GetControllerContext());
        }
        public static ControllerContext GetControllerContext() {
            return GetControllerContext(null);
        }
        public static ControllerContext GetControllerContext(NameValueCollection form) {
            var mockHttpContext = GetHttpContext(form);
            var controllerContext = new ControllerContext(mockHttpContext, new RouteData(), new Mock<ControllerBase>().Object);
            controllerContext.Controller.TempData = new TempDataDictionary();
            controllerContext.Controller.TempData.Add(Honeypot.TempDataKey, FakeInputName);
            return controllerContext;
        }
    }
}