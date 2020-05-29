using System;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class TransactionMapperProfileTests
    {
        [Fact]
        public void MapTransactionDtoToTransaction()
        {
            // Arrange
            var transactionDto = new TransactionDto
            {
                ReferenceNumber = "rrn",
                TotalPrice = 100,
                Date = DateTime.Now
            };

            // Act
            var transaction = AutoMapperConfiguration.Mapper.Map<TransactionDto, Transaction>(transactionDto);

            // Assert
            Assert.Equal(transactionDto.ReferenceNumber, transaction.ReferenceNumber);
            Assert.Equal(transactionDto.TotalPrice, transaction.TotalPrice);
            Assert.Equal(transactionDto.Date, transaction.Date);
        }

        [Fact]
        public void MapTransactionToTransactionDto()
        {
            // Arrange
            var transaction = new Transaction
            {
                ReferenceNumber = "rrn",
                TotalPrice = 100,
                Date = DateTime.Now
            };

            // Act
            var transactionDto = AutoMapperConfiguration.Mapper.Map<Transaction, TransactionDto>(transaction);

            // Assert
            Assert.Equal(transaction.ReferenceNumber, transactionDto.ReferenceNumber);
            Assert.Equal(transaction.TotalPrice, transactionDto.TotalPrice);
            Assert.Equal(transaction.Date, transactionDto.Date);
        }
    }
}