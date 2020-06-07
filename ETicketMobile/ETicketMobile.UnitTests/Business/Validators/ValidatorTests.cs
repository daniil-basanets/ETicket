using ETicketMobile.Business.Validators;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Validators
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("b@gmail.com")]
        [InlineData("anton.ukrainets@gmail.com")]
        public void CheckHasEmailCorrectLength_ReturnsTrue(string email)
        {
            // Act
            var actualValue = Validator.HasEmailCorrectLength(email);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("qwertyuiopasdfghkjklzxcvvcbmvnbzxczxczadashrtwreqwqe@gmail.com")]
        public void CheckHasEmailCorrectLength_ReturnsFalse(string email)
        {
            // Act
            var actualValue = Validator.HasEmailCorrectLength(email);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1234567")]
        public void CheckIfPasswordShort_ReturnsTrue(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordShort(password);

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CheckIfPasswordShort_ReturnsFalse()
        {
            // Arrange
            var password = "asdasdasdasdasdasdasdasddasdasdasdasasdasdasd123abc" +
                           "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdassass";

            // Act
            var actualValue = Validator.IsPasswordShort(password);

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CheckIfPasswordLong_ReturnsTrue()
        {
            // Arrange
            var password = "asdasdasdasdasdasdasdasddasdasdasdasasdasdasd123abc" +
                           "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdassass";

            // Act
            var actualValue = Validator.IsPasswordLong(password);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1234567")]
        public void CheckIfPasswordLong_ReturnsFalse(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordLong(password);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("123123")]
        [InlineData("12311231231223")]
        public void CheckIfPasswordWeak_ReturnsTrue(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordWeak(password);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("abc")]
        [InlineData("qwerty12")]
        public void CheckIfPasswordWeak_ReturnsFalse(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordWeak(password);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty12")]
        [InlineData("abcdefgh", "abcdefgh")]
        public void CheckIfPasswordsMatched_ReturnsTrue(string password, string confirmPassword)
        {
            // Act
            var actualValue = Validator.PasswordsMatched(password, confirmPassword);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty21")]
        [InlineData("1234567A", "1234567a")]
        public void CheckIfPasswordsMatched_ReturnsFalse(string password, string confirmPassword)
        {
            // Act
            var actualValue = Validator.PasswordsMatched(password, confirmPassword);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("1234432156788765")]
        public void CheckHasCardNumberCorrectLength_ReturnsTrue(string cardNumber)
        {
            // Act
            var actualValue = Validator.HasCardNumberCorrectLength(cardNumber);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("abc")]
        [InlineData("1234 4321 5678 8765")]
        public void CheckHasCardNumberCorrectLength_ReturnsFalse(string cardNumber)
        {
            // Act
            var actualValue = Validator.HasCardNumberCorrectLength(cardNumber);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("01/12")]
        [InlineData("00/00")]
        public void CheckHasExpirationDateCorrectLength_ReturnsTrue(string expirationDate)
        {
            // Act
            var actualValue = Validator.HasExpirationDateCorrectLength(expirationDate);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("0112")]
        [InlineData("01/123")]
        public void CheckHasExpirationDateCorrectLength_ReturnsFalse(string expirationDate)
        {
            // Act
            var actualValue = Validator.HasExpirationDateCorrectLength(expirationDate);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("810")]
        [InlineData("121")]
        public void CheckHasCVV2CorrectLength_ReturnsTrue(string cvv2)
        {
            // Act
            var actualValue = Validator.HasCVV2CorrectLength(cvv2);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("1234")]
        public void CheckHasCVV2CorrectLength_ReturnsFalse(string cvv2)
        {
            // Act
            var actualValue = Validator.HasCVV2CorrectLength(cvv2);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("Mo")]
        [InlineData("Wolfe­schlegel­stein­haus")]
        public void CheckIfNameValid_ReturnsTrue(string name)
        {
            // Act
            var actualValue = Validator.IsNameValid(name);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("Wolfe­schlegel­stein­hausen­berger­dorff")]
        public void CheckIfNameValid_ReturnsFalse(string name)
        {
            // Act
            var actualValue = Validator.IsNameValid(name);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("+380679953365")]
        public void CheckHasPhoneCorrectLength_ReturnsTrue(string name)
        {
            // Act
            var actualValue = Validator.HasPhoneCorrectLength(name);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0679953365")]
        [InlineData("+38 (067)-995-33-65")]
        public void CheckHasPhoneCorrectLength_ReturnsFalse(string name)
        {
            // Act
            var actualValue = Validator.HasPhoneCorrectLength(name);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void CheckIfTicketChoosed_ReturnsTrue(int countTickets)
        {
            // Act
            var actualValue = Validator.TicketChoosed(countTickets);

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CheckIfTicketChoosed_ReturnsFalse()
        {
            // Arrange
            var countTickets = 0;

            // Act
            var actualValue = Validator.TicketChoosed(countTickets);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void CheckIfAreaChoosed_ReturnsTrue(int countAreas)
        {
            // Act
            var actualValue = Validator.AreaChoosed(countAreas);

            // Assert
            Assert.True(actualValue);
        }

        [Fact]
        public void CheckIfAreaChoosed_ReturnsFalse()
        {
            // Arrange
            var countAreas = 0;

            // Act
            var actualValue = Validator.AreaChoosed(countAreas);

            // Assert
            Assert.False(actualValue);
        }
    }
}