namespace SimpleHoneypot.Tests {
    using SimpleHoneypot.Core;
    using System.Collections.Specialized;
    using Xunit;

    public class HoneypotWorkerTests {
        #region Public Methods and Operators

        [Fact]
        public void GetHoneypotData_ShouldReturnHoneypotDefaultInputValue_WhenInputNamesCollectionCountIsLessThanTwo() {
            // Arrange
            var worker = new HoneypotWorker();
            Honeypot.InputNames.Clear();
            
            //Act
            HoneypotData token = worker.GetHoneypotData(MvcHelper.GetHttpContext());

            //Assert
            Assert.Equal(token.InputNameValue, HoneypotData.DefaultFieldName);
        }

        [Fact]
        public void GetHoneypotData_ShouldReturnRandomInputName_WhenInputNamesCollectionCountIsGreaterThanOne() {
            // Arrange
            var worker = new HoneypotWorker();
            Honeypot.InputNames.Add("test");
            Honeypot.InputNames.Add("name");

            // Act
            HoneypotData token = worker.GetHoneypotData(MvcHelper.GetHttpContext());

            // Assert
            Assert.NotEqual(token.InputNameValue, HoneypotData.DefaultFieldName);
        }

        [Fact]
        public void IsBot_ShouldBeTrue_IfTokenInputIsNotInForm() {
            // Arrang
            var worker = new HoneypotWorker();
            var form = new NameValueCollection { { "FirstName", "Jon" } };

            // Act
            bool isBot = worker.IsBot(MvcHelper.GetHttpContext(form));

            // Assert
            Assert.True(isBot);
        }

        [Fact]
        public void IsBot_ShouldBeTrue_IfHpInputNameIsNotInForm()
        {
            // Arrang
            Honeypot.InputNames.Clear();
            var worker = new HoneypotWorker();
            var token = new HoneypotData(HoneypotData.DefaultFieldName);
            var serializer = new HoneypotDataSerializer();
            var form = new NameValueCollection {
                { "FirstName", "Jon" }, {
                    HoneypotData.FormKeyFieldName,
                    serializer.Serialize(token)
                }
            };

            // Act
            bool isBot = worker.IsBot(MvcHelper.GetHttpContext(form));

            // Assert
            Assert.True(isBot);
        }

        [Fact]
        public void IsBot_ShouldBeFalse_IfHpInputNameIsInFormAndEmpty()
        {
            // Arrang
            Honeypot.InputNames.Clear();
            var worker = new HoneypotWorker();
            var token = new HoneypotData(HoneypotData.DefaultFieldName);
            var serializer = new HoneypotDataSerializer();
            var form = new NameValueCollection {
                { "FirstName", "Jon" }, {
                    HoneypotData.FormKeyFieldName,
                    serializer.Serialize(token)
                },
                {token.InputNameValue, ""}
            };

            // Act
            bool isBot = worker.IsBot(MvcHelper.GetHttpContext(form));

            // Assert
            Assert.False(isBot);
        }

        [Fact]
        public void IsBot_ShouldBeTrue_IfHpInputNameIsInFormAndHasValue()
        {
            // Arrang
            Honeypot.InputNames.Clear();
            var worker = new HoneypotWorker();
            var token = new HoneypotData(HoneypotData.DefaultFieldName);
            var serializer = new HoneypotDataSerializer();
            var form = new NameValueCollection {
                { "FirstName", "Jon" }, {
                    HoneypotData.FormKeyFieldName,
                    serializer.Serialize(token)
                }, {token.InputNameValue, "I am a bot"}
            };

            // Act
            bool isBot = worker.IsBot(MvcHelper.GetHttpContext(form));

            // Assert
            Assert.True(isBot);
        }

        [Fact]
        public void GetHtml_ShouldSetTheCssClassNameOnTrapInput() {
            // Arrange
            var worker = new HoneypotWorker();

            //Act
            string html = worker.GetHtml(MvcHelper.GetHtmlHelper(), MvcHelper.GetHttpContext()).ToString();

            // Assert
            Assert.True(html.Contains(Honeypot.CssClassName));
        }

        #endregion
    }
}