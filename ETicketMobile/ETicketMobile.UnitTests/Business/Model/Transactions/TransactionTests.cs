using System;
using ETicketMobile.Business.Model.Transactions;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Transactions
{
    public class TransactionTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var referenceNumber = "rrn";
            var totalPrice = 100;
            var date = DateTime.Now;

            // Act
            var transaction = new Transaction
            {
                ReferenceNumber = referenceNumber,
                TotalPrice = totalPrice,
                Date = date
            };

            // Assert
            Assert.Equal(referenceNumber, transaction.ReferenceNumber);
            Assert.Equal(totalPrice, transaction.TotalPrice);
            Assert.Equal(date, transaction.Date);
        }
    }
}