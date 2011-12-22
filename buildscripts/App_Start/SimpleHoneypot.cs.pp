using System.Web;
using System.Web.Mvc;
using SimpleHoneypot.Core;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.SimpleHoneypot), "Start")]

namespace $rootnamespace$.App_Start {
    public static class SimpleHoneypot {
        public static void Start() {
            RegisterHoneypotInputNames(Honeypot.InputNames);
        }
		
		public static void RegisterHoneypotInputNames(HoneypotInputNameCollection collection) {
            //Honeypot will use 2 words at random to create the input name {0}-{1}
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
        }
    }
}