using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class ConfirmEmailResponseDtoTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckInstanceProperties(bool succeeded)
        {
            // Act
            var confirmEmailResponseDto = new ConfirmEmailResponseDto
            {
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(succeeded, confirmEmailResponseDto.Succeeded);
        }
    }
}