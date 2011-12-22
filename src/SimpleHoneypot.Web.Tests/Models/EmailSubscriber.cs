using System.ComponentModel.DataAnnotations;

namespace SimpleHoneypot.Web.Tests.Models {
    public class EmailSubscriber {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}