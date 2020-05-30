using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class CreateNewPasswordResponseDtoTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckInstanceProperties(bool succeeded)
        {
            // Act
            var createNewPasswordResponseDto = new CreateNewPasswordResponseDto
            {
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(succeeded, createNewPasswordResponseDto.Succeeded);
        }
    }
}