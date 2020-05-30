using System;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class TransactionDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var referenceNumber = "ReferenceNumber";
            var totalPrice = 100;
            var date = DateTime.Now;

            // Act
            var transactionDto = new TransactionDto
            {
                ReferenceNumber = referenceNumber,
                TotalPrice = totalPrice,
                Date = date
            };

            // Assert
            Assert.Equal(referenceNumber, transactionDto.ReferenceNumber);
            Assert.Equal(totalPrice, transactionDto.TotalPrice);
            Assert.Equal(date, transactionDto.Date);
        }
    }
}