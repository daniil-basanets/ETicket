using System;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class BuyTicketResponseDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var payResult = "PayResult";
            var totalPrice = 100;
            var boughtAt = DateTime.Now;

            // Act
            var buyTicketResponseDto = new BuyTicketResponseDto
            {
                PayResult = payResult,
                TotalPrice = totalPrice,
                BoughtAt = boughtAt
            };

            // Assert
            Assert.Equal(payResult, buyTicketResponseDto.PayResult);
            Assert.Equal(totalPrice, buyTicketResponseDto.TotalPrice);
            Assert.Equal(boughtAt, buyTicketResponseDto.BoughtAt);
        }
    }
}