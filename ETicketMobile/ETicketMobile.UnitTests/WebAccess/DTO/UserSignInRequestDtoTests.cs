using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class UserSignInRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";
            var password = "Password";
            var errorMessage = "ErrorMessage";

            // Act
            var userSignInRequestDto = new UserSignInRequestDto
            {
                Email = email,
                Password = password,
                ErrorMessage = errorMessage
            };

            // Assert
            Assert.Equal(email, userSignInRequestDto.Email);
            Assert.Equal(password, userSignInRequestDto.Password);
            Assert.Equal(errorMessage, userSignInRequestDto.ErrorMessage);
        }
    }
}