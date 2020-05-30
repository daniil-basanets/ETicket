using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class CreateNewPasswordRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";
            var newPassword = "NewPassword";
            var resetPasswordCode = "ResetPasswordCode";

            // Act
            var createNewPasswordRequestDto = new CreateNewPasswordRequestDto
            {
                Email = email,
                NewPassword = newPassword,
                ResetPasswordCode = resetPasswordCode
            };

            // Assert
            Assert.Equal(email, createNewPasswordRequestDto.Email);
            Assert.Equal(newPassword, createNewPasswordRequestDto.NewPassword);
            Assert.Equal(resetPasswordCode, createNewPasswordRequestDto.ResetPasswordCode);
        }
    }
}