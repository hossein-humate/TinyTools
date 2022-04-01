using Basemap.Identity.Common;
using System.Security.Cryptography;
using Xunit;

namespace Passworder.Tests
{
    /// <summary>
    /// Test suite for <see cref="PasswordCheck"/>.
    /// </summary>
    public class PasswordCheckTests
    {
        private readonly IPasswordCheck _passwordCheck;

        /// <summary>
        /// Initialises an instance of <see cref="PasswordCheckTests"/>.
        /// </summary>
        public PasswordCheckTests()
        {
            _passwordCheck = new PasswordCheck();
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasDigit(string)" />
        /// check the given string value has any digit
        /// </summary>
        [Fact]
        public void HasDigit_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "a!sd12@xyZ";

            //Act
            var result = _passwordCheck.HasDigit(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasMinLength(string, int)" />
        /// check the given string value has minimum 8 character
        /// </summary>
        [Fact]
        public void HasMinLength_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "Abc!12@xyZ";

            //Act
            var result = _passwordCheck.HasMinLength(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasMinLength(string, int)" />
        /// check the given string value has Maximum 20 character
        /// </summary>
        [Fact]
        public void HasMaxLength_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "abcdEFG234123456789";

            //Act
            var result = _passwordCheck.HasMaxLength(input, 20);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasLowerCase(string)" />
        /// check the given string value has at least one lower case character
        /// </summary>
        [Fact]
        public void HasLowerCase_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "1234567aA";

            //Act
            var result = _passwordCheck.HasLowerCase(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasUpperCase(string)" />
        /// check the given string value has at least one upper case character
        /// </summary>
        [Fact]
        public void HasUpperCase_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "1234567aA";

            //Act
            var result = _passwordCheck.HasUpperCase(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasSpecialChar(string)" />
        /// check the given string value has at least one special character
        /// </summary>
        [Fact]
        public void HasSpecialChar_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "@!#";

            //Act
            var result = _passwordCheck.HasSpecialChar(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasSpaceChar(string)" />
        /// check the given string value has at least one space character
        /// </summary>
        [Fact]
        public void HasSpaceChar_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "abc d  ft";

            //Act
            var result = _passwordCheck.HasSpaceChar(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.HasFullComplexity(string)" />
        /// check the given string value pass full complexity
        /// </summary>
        [Fact]
        public void HasFullComplexity_ShouldReturnTrue_WhenGivenPassword()
        {
            //Arrange
            var input = "Abc!12@xyZ";

            //Act
            var result = _passwordCheck.HasFullComplexity(input);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Asserts that
        /// <see cref = "IPasswordCheck.NotEqual(string, string, string)" />
        /// check the given new password value not same as the old one
        /// </summary>
        [Theory]
        [InlineData("ABCabc!123@", "ABCabc!123@")]
        [InlineData("abcABC!123@", "ABCabc!123@")]
        public void NotEqual_ShouldReturnTrueOrFalse_DependOnGivenPassword(string newPass, string oldPass)
        {
            //Arrange
            var salt = PasswordCheck.GenerateSecret();
            var hash = PasswordCheck.CreateHash(oldPass,salt);

            //Act
            var result = _passwordCheck.NotEqual(newPass, salt, hash);

            //Assert
            if (newPass == oldPass)
            {
                Assert.False(result);
            }
            else
            {
                Assert.True(result);
            }
        }
    }
}
