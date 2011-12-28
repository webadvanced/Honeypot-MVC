using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Web.Tests {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        public static void RegisterHoneypotInputNames(HoneypotInputNameCollection collection) {
            //Honeypot will use 2 words to create the input name {0}-{1}
            collection.Add(new[]
                           {
                               "User",
                               "Name",
                               "Age",
                               "Question",
                               "List",
                               "Why",
                               "Type",
                               "Phone",
                               "Fax",
                               "Custom",
                               "Relationship",
                               "Friend",
                               "Pet",
                               "Reason"
                           });
            //Honeypot.SetManuallyHandleBots(true);
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterHoneypotInputNames(Honeypot.InputNames);
        }
    }
}