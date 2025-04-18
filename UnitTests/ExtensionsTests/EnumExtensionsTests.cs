namespace UnitTests.ExtensionsTests
{
    using System.ComponentModel;
    using YourWheel.Domain.Extensions;

    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [AmbientValue(typeof(Guid), "21e4bd82-89ea-48c1-8923-4fd9b3df7a17")]
            Guid,
            [AmbientValue(123)]
            Int,
            [AmbientValue("qwerty")]
            String,
            [AmbientValue(null)]
            Null,
            Without
        }

        [Fact]
        public void GetAmbientValue_ReturnGuidValue()
        {
            // Act
            var result = TestEnum.Guid.GetAmbientValue();

            // Assert
            Assert.Equal(Guid.Parse("21e4bd82-89ea-48c1-8923-4fd9b3df7a17"), result);
        }

        [Fact]
        public void GetAmbientValue_ReturnIntValue()
        {
            // Act
            var result = TestEnum.Int.GetAmbientValue();

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void GetAmbientValue_ReturnStringValue()
        {
            // Act
            var result = TestEnum.String.GetAmbientValue();

            // Assert
            Assert.Equal("qwerty", result);
        }

        [Fact]
        public void GetAmbientValue_ReturnNull()
        {
            // Act
            var result = TestEnum.Null.GetAmbientValue();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAmbientValue_AttributeAbsent_ReturnDefault()
        {
            // Act
            var result = TestEnum.Without.GetAmbientValue();

            // Assert
            Assert.Null(result);
        }
    }
}
