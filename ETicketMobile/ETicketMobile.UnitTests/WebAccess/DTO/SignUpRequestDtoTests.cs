using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class SignUpRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";

            // Act
            var signUpRequestDto = new SignUpRequestDto
            {
                Email = email
            };

            // Assert
            Assert.Equal(email, signUpRequestDto.Email);
        }
    }
}