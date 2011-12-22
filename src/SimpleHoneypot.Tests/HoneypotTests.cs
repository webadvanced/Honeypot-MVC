using Xunit;
using SimpleHoneypot.Core;

namespace SimpleHoneypot.Tests {
    public class HoneypotTests {
        [Fact]
        public void SetCssClassName_ShouldSetTheCssClassNameProperty_WhenPassedAValidString() {
            var str = "MockCssClass";

            Assert.Equal("input-imp-long", Honeypot.CssClassName);
            Honeypot.SetCssClassName(str);

            Assert.Equal(str, Honeypot.CssClassName);
        } 
    }
}