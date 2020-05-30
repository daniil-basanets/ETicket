using ETicketMobile.Business.Validators;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Validators
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("b@gmail.com")]
        [InlineData("anton.ukrainets@gmail.com")]
        public void HasEmailCorrectLength_ReturnsTrue(string email)
        {
            // Act
            var actualValue = Validator.HasEmailCorrectLength(email);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("qwertyuiopasdfghkjklzxcvvcbmvnbzxczxczadashrtwreqwqe@gmail.com")]
        public void HasEmailCorrectLength_ReturnsFalse(string email)
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
        public void IsPasswordShort_ReturnsTrue(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordShort(password);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("12345612321312312312321323")]
        [InlineData("ABCABCABCC")]
        public void IsPasswordShort_ReturnsFalse(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordShort(password);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("asdasdasdasdasdasdasdasddasdasdasdasasdasdasd" +
                    "asdasdasdasdasddasdasdasdasasdasdasdasdasdasdasdasdaasss")]
        public void IsPasswordLong_ReturnsTrue(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordLong(password);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1234567")]
        public void IsPasswordLong_ReturnsFalse(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordLong(password);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("123123")]
        [InlineData("12311231231223")]
        public void IsPasswordWeak_ReturnsTrue(string password)
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
        public void IsPasswordWeak_ReturnsFalse(string password)
        {
            // Act
            var actualValue = Validator.IsPasswordWeak(password);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty12")]
        [InlineData("abcdefgh", "abcdefgh")]
        public void PasswordsMatched_ReturnsTrue(string password, string confirmPassword)
        {
            // Act
            var actualValue = Validator.PasswordsMatched(password, confirmPassword);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData("qwerty12", "qwerty21")]
        [InlineData("1234567A", "1234567a")]
        public void PasswordsMatched_ReturnsFalse(string password, string confirmPassword)
        {
            // Act
            var actualValue = Validator.PasswordsMatched(password, confirmPassword);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("1234432156788765")]
        public void HasCardNumberCorrectLength_ReturnsTrue(string cardNumber)
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
        public void HasCardNumberCorrectLength_ReturnsFalse(string cardNumber)
        {
            // Act
            var actualValue = Validator.HasCardNumberCorrectLength(cardNumber);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("01/12")]
        [InlineData("00/00")]
        public void HasExpirationDateCorrectLength_ReturnsTrue(string expirationDate)
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
        public void HasExpirationDateCorrectLength_ReturnsFalse(string expirationDate)
        {
            // Act
            var actualValue = Validator.HasExpirationDateCorrectLength(expirationDate);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("810")]
        [InlineData("121")]
        public void HasCVV2CorrectLength_ReturnsTrue(string cvv2)
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
        public void HasCVV2CorrectLength_ReturnsFalse(string cvv2)
        {
            // Act
            var actualValue = Validator.HasCVV2CorrectLength(cvv2);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("Mo")]
        [InlineData("Wolfe­schlegel­stein­haus")]
        public void IsNameValid_ReturnsTrue(string name)
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
        public void IsNameValid_ReturnsFalse(string name)
        {
            // Act
            var actualValue = Validator.IsNameValid(name);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData("+380679953365")]
        public void HasPhoneCorrectLength_ReturnsTrue(string name)
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
        public void HasPhoneCorrectLength_ReturnsFalse(string name)
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
        public void TicketChoosed_ReturnsTrue(int countTickets)
        {
            // Act
            var actualValue = Validator.TicketChoosed(countTickets);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(0)]
        public void TicketChoosed_ReturnsFalse(int countTickets)
        {
            // Act
            var actualValue = Validator.TicketChoosed(countTickets);

            // Assert
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void AreaChoosed_ReturnsTrue(int countAreas)
        {
            // Act
            var actualValue = Validator.AreaChoosed(countAreas);

            // Assert
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(0)]
        public void AreaChoosed_ReturnsFalse(int countAreas)
        {
            // Act
            var actualValue = Validator.AreaChoosed(countAreas);

            // Assert
            Assert.False(actualValue);
        }
    }
}