using Flashcards.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services
{
    public class UserInputValidatorTests
    {
        public static IEnumerable<object[]> MakeString()
        {
            yield return new object[] { new string('a', 26) };
            yield return new object[] { new string('a', 255) };
        }

        public static IEnumerable<object[]> MakeTooLongFlashcardTextField()
        {
            yield return new object[] { new string('a', 101) };
            yield return new object[] { new string('a', 255) };
        }

        public static IEnumerable<object[]> MakeCorrectFlashcardTextField()
        {
            yield return new object[] { new string('a', 100) };
            yield return new object[] { "a" };
            yield return new object[] { "deck" };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("김치")]
        public void ValidateDeckName_WithTooShortDeckName_ShouldReturnOne(string value)
        {
            Assert.Equal(1, UserInputValidator.ValidateDeckName(value));
        }

        [Theory]
        [MemberData(nameof(MakeString))]
        public void ValidateDeckName_WithTooLongDeckName_ShouldReturnTwo(string value)
        {
            Assert.Equal(2, UserInputValidator.ValidateDeckName(value));
        }

        [Theory]
        [InlineData("deck")]
        [InlineData("deckdeckdeckdeckdeckdeckd")]
        [InlineData("aaa")]
        public void ValidateDeckName_WithCorrectDeckName_ShouldReturnZero(string value)
        {
            Assert.Equal(0, UserInputValidator.ValidateDeckName(value));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ValidateFlashcardTextField_WithTooShortText_ShouldReturnOne(string value)
        {
            Assert.Equal(1, UserInputValidator.ValidateFlashcardTextField(value));
        }

        [Theory]
        [MemberData(nameof(MakeTooLongFlashcardTextField))]
        public void ValidateFlashcardTextField_WithTooLongText_ShouldReturnTwo(string value)
        {
            Assert.Equal(2, UserInputValidator.ValidateFlashcardTextField(value));
        }

        [Theory]
        [MemberData(nameof(MakeCorrectFlashcardTextField))]
        public void ValidateFlashcardTextField_WithCorrectText_ShouldReturnZero(string value)
        {
            Assert.Equal(0, UserInputValidator.ValidateFlashcardTextField(value));
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("aaa")]
        public void ValidateUsername_WithTooShortUsername_ShouldReturnOne(string value)
        {
            Assert.Equal(1, UserInputValidator.ValidateUsername(value));
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaa")] // length = 26
        [InlineData("ضضضضضضضضضضضضضضضضضضضضضضضضضض")] //length = 26
        public void ValidateUsername_WithTooLongUsername_ShouldReturnTwo(string value)
        {
            Assert.Equal(2, UserInputValidator.ValidateUsername(value));
        }

        [Theory]
        [InlineData("test ")]
        [InlineData(" test")]
        [InlineData("te st")]
        [InlineData("    ")]
        [InlineData("\ttest")]
        [InlineData("test\n")]
        public void ValidateUsername_WithWhiteSpaces_ShouldReturnThree(string value)
        {
            Assert.Equal(3, UserInputValidator.ValidateUsername(value));
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaa")] //length = 25
        [InlineData("test")]
        public void ValidateUsername_WithCorrectUsername_ShouldReturnZero(string value)
        {
            Assert.Equal(0, UserInputValidator.ValidateUsername(value));
        }

        [Theory]
        [InlineData("aaaaaaa")]
        [InlineData(null)]
        [InlineData("")]
        public void ValidatePassword_WithTooShortArg_ShouldReturnOne(string value)
        {
            Assert.Equal(1, UserInputValidator.ValidatePassword(value));
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaa")] // length = 26
        [InlineData("ضضضضضضضضضضضضضضضضضضضضضضضضضض")] //length = 26
        public void ValidatePassword_WithTooLongArg_ShouldReturnTwo(string value)
        {
            Assert.Equal(2, UserInputValidator.ValidatePassword(value));
        }

        [Theory]
        [InlineData("        test")]
        [InlineData("\ttesttest")]
        [InlineData("\ntesttest")]
        public void ValidatePassword_WithWhiteSpaces_ShouldReturnThree(string value)
        {
            Assert.Equal(3, UserInputValidator.ValidatePassword(value));
        }

        [Theory]
        [InlineData("ضaaaaaaa")]
        public void ValidatePassword_WithUnicodeCharacters_ShouldReturnFour(string value)
        {
            Assert.Equal(4, UserInputValidator.ValidatePassword(value));
        }

        [Theory]
        [InlineData("testtest")]
        [InlineData("12345678")]
        [InlineData("aaa$aaaaaaaaaaaAaa5aaaaaa")] //length = 25
        public void ValidatePassword_WithCorrectArg_ShouldReturnZero(string value)
        {
            Assert.Equal(0, UserInputValidator.ValidatePassword(value));
        }

        [Theory]
        [InlineData("a@a")]
        [InlineData("hah@haha.com.pl")]
        [InlineData("1$3@1313")]
        public void IsValidEmail_WithCorrectEmail_ShouldReturnTrue(string value)
        {
            Assert.True(UserInputValidator.IsValidEmail(value));
        }

        [Theory]
        [InlineData("haha")]
        [InlineData("haha.haha")]
        [InlineData(null)]
        [InlineData("   @   .pl")]
        public void IsValidEmail_WithInvalidEmail_ShouldReturnFalse(string value)
        {
            Assert.False(UserInputValidator.IsValidEmail(value));
        }
    }
}
