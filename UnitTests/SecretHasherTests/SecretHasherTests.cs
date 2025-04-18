namespace UnitTests.SecretHasherTests
{
    using YourWheel.Host.Helpers;

    public class SecretHasherTests
    {
        [Fact]
        public void GetHash()
        {
            // Arrange
            string input = "qwerty";

            // Act
            string hash = SecretHasher.Hash(input);

            // Assert
            Assert.NotEmpty(hash);

            Assert.NotEqual(input, hash);
        }

        [Fact]
        public void VerifyInput_ReturnTrue()
        {
            // Arrange
            string input = "qwerty";

            string hash = SecretHasher.Hash(input);

            // Act
            bool result = SecretHasher.Verify(input, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyInvalidInput_ReturnsFalse()
        {
            // Arrange
            string input = "qwerty";

            string hash = SecretHasher.Hash(input);

            string invalidInput = "invalid";

            // Act
            bool result = SecretHasher.Verify(invalidInput, hash);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DifferentHashesForDifferentInputs_ReturnFalse()
        {
            // Arrange
            string input1 = "qwerty";

            string input2 = "ytrewq";

            // Act
            string hash1 = SecretHasher.Hash(input1);

            string hash2 = SecretHasher.Hash(input2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void OneInputForDifferentHashes_ReturnsFalse()
        {
            // Arrange
            string input = "qwerty";

            // Act
            string hash1 = SecretHasher.Hash(input);

            string hash2 = SecretHasher.Hash(input);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
