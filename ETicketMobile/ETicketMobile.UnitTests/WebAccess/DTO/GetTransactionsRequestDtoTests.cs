using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class GetTransactionsRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";

            // Act
            var getTransactionRequestDto = new GetTransactionsRequestDto
            {
                Email = email
            };

            // Assert
            Assert.Equal(email, getTransactionRequestDto.Email);
        }
    }
}