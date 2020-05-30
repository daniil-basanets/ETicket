using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class SignUpResponseDtoTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckInstanceProperties(bool succeeded)
        {
            // Act
            var signUpRequestDto = new SignUpResponseDto
            {
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(succeeded, signUpRequestDto.Succeeded);
        }
    }
}