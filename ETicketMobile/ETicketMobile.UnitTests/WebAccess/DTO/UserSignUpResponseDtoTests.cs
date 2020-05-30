using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class UserSignUpResponseDtoTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckInstanceProperties(bool succeeded)
        {
            // Act
            var userSignUpResponseDto = new UserSignUpResponseDto
            {
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(succeeded, userSignUpResponseDto.Succeeded);
        }
    }
}