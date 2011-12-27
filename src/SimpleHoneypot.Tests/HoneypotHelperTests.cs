using SimpleHoneypot.Core;
using SimpleHoneypot.HtmlHelpers;
using Xunit;

namespace SimpleHoneypot.Tests {
    public class HoneypotHelperTests {
        [Fact]
        public void HoneypotInput_ShouldUseDefaultInputName_WhenHoneypotInputNamesCollectionHasLessThenTwoItems() {
            Honeypot.InputNames.Clear();
            var htmlHelper = MvcHelper.GetHtmlHelper();
            var honeypotImput = htmlHelper.HoneypotInput();
            var defaultInputName = Honeypot.DefaultInputName;
            Assert.True(honeypotImput.ToString().Contains(defaultInputName));
        }

        [Fact]
        public void HoneypotInput_ShouldGenerateRandomInputName_WhenHoneypotInputNamesCollectionHasTwoOrMoreItems() {
            Honeypot.InputNames.Add(new [] {"MockName1", "MockName2"});
            var htmlHelper = MvcHelper.GetHtmlHelper();
            var honeypotImput = htmlHelper.HoneypotInput();
            var defaultInputName = Honeypot.DefaultInputName;
            Assert.False(honeypotImput.ToString().Contains(defaultInputName));
            Assert.True(honeypotImput.ToString().Contains("MockName1"));
            Assert.True(honeypotImput.ToString().Contains("MockName2"));
        }

        [Fact]
        public void HoneypotInput_ShouldUseHoneyPotCssClassName() {
            var htmlHelper = MvcHelper.GetHtmlHelper();
            var honeypotImput = htmlHelper.HoneypotInput();
            var cssClassName = Honeypot.CssClassName;
            Assert.True(honeypotImput.ToString().Contains(cssClassName));
        }
    }
}