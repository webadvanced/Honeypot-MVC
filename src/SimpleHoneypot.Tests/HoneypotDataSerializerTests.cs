namespace SimpleHoneypot.Tests {
    using System;

    using SimpleHoneypot.Core;

    using Xunit;

    public class HoneypotDataSerializerTests {
        #region Public Methods and Operators

        [Fact]
        public void CanRoundTripData() {
            // Arrange
            var serializer = new HoneypotDataSerializer
            { Decoder = value => Convert.FromBase64String(value), Encoder = bytes => Convert.ToBase64String(bytes) };

            var input = new HoneypotData("input-name");

            // Act
            HoneypotData output = serializer.Deserialize(serializer.Serialize(input));

            // Assert
            Assert.NotNull(output);
            Assert.Equal(input.InputNameValue, output.InputNameValue);
            Assert.Equal(input.Key, output.Key);
            Assert.Equal(input.CreationDate, output.CreationDate);
        }

        [Fact]
        public void GuardClauses() {
            // Arrange
            var serializer = new HoneypotDataSerializer();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => serializer.Serialize(null));

            Assert.Throws<ArgumentException>(() => serializer.Deserialize(null));

            Assert.Throws<ArgumentException>(() => serializer.Deserialize(String.Empty));

            Assert.Throws<InvalidOperationException>(() => serializer.Deserialize("Corrupted Base-64 Value"));
        }

        #endregion
    }
}