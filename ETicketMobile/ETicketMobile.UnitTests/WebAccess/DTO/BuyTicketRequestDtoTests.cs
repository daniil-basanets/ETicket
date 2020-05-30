using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class BuyTicketRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var ticketTypeId = 1;
            var email = "Email";
            var description = "Description";
            var price = 100;
            var cardNumber = "CardNumber";
            var expirationMonth = "ExpirationMonth";
            var expirationYear = "ExpirationYear";
            var cvv2 = "CVV2";

            // Act
            var buyTicketRequestDto = new BuyTicketRequestDto
            {
                TicketTypeId = ticketTypeId,
                Email = email,
                Description = description,
                Price = price,
                CardNumber = cardNumber,
                ExpirationMonth = expirationMonth,
                ExpirationYear = expirationYear,
                CVV2 = cvv2
            };

            // Assert
            Assert.Equal(ticketTypeId, buyTicketRequestDto.TicketTypeId);
            Assert.Equal(email, buyTicketRequestDto.Email);
            Assert.Equal(description, buyTicketRequestDto.Description);
            Assert.Equal(price, buyTicketRequestDto.Price);
            Assert.Equal(cardNumber, buyTicketRequestDto.CardNumber);
            Assert.Equal(expirationMonth, buyTicketRequestDto.ExpirationMonth);
            Assert.Equal(expirationYear, buyTicketRequestDto.ExpirationYear);
            Assert.Equal(cvv2, buyTicketRequestDto.CVV2);
        }
    }
}