using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Tests {
    public static class MvcHelper {
        public const string AppPathModifier = "/$(SESSION)";

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
                                  TempData = new TempDataDictionary()
                              };
            var mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);
            
            var htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);
            return htmlHelper;
        }

        public static HtmlHelper GetHtmlHelper(string protocol, int port) {
            HttpContextBase httpcontext = GetHttpContext("/app/", null, null, protocol, port);
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
                                                     string protocol, int port) {
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

            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase) null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(
                r => AppPathModifier + r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, string requestPath, string httpMethod) {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp, -1);
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

        public static AuthorizationContext BuildAuthorizationContext(bool addFormValue) {
            string fakeInputName = "Fake-Input-Name";
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("POST");
            request.Setup(r => r.Headers).Returns(new NameValueCollection());
            request.Setup(r => r.Form).Returns(new NameValueCollection());
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());
            request.Setup(r => r.Files).Returns(new Mock<HttpFileCollectionBase>().Object);

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Expect(c => c.Request).Returns(request.Object);
            mockHttpContext.Setup(c => c.Session).Returns((HttpSessionStateBase)null);
            if (addFormValue) {
                var form = new NameValueCollection { { fakeInputName, "I Am A Spam Bot!" } };
                mockHttpContext.Setup(c => c.Request.Form).Returns(form);
            }

            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            controllerContext.Controller.TempData = new TempDataDictionary();
            controllerContext.Controller.TempData.Add(Honeypot.TempDataKey, "Fake-Input-Name");


            return new AuthorizationContext(controllerContext);
        }
    }
}