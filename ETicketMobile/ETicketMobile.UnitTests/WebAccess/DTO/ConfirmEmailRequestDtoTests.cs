using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class ConfirmEmailRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";
            var activationCode = "ActivationCode";

            // Act
            var confirmEmailRequestDto = new ConfirmEmailRequestDto
            {
                Email = email,
                ActivationCode = activationCode
            };

            // Assert
            Assert.Equal(email, confirmEmailRequestDto.Email);
            Assert.Equal(activationCode, confirmEmailRequestDto.ActivationCode);
        }
    }
}