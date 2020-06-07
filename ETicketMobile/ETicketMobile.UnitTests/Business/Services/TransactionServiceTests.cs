using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.Business.Services;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class TransactionServiceTests
    {
        #region Fields

        private readonly ITransactionService transactionService;

        private readonly Mock<IHttpService> httpServiceMock;

        private readonly IEnumerable<TransactionDto> transactionsDto;
        private readonly IEnumerable<Transaction> transactions;

        private readonly string email;

        #endregion

        public TransactionServiceTests()
        {
            email = "email";

            transactionsDto = new List<TransactionDto>
            {
                new TransactionDto
                {
                    ReferenceNumber = "ReferenceNumber1",
                    TotalPrice = 101,
                    Date = DateTime.Parse("06/06/20 18:01:00")
                },
                new TransactionDto
                {
                    ReferenceNumber = "ReferenceNumber2",
                    TotalPrice = 102,
                    Date = DateTime.Parse("06/06/20 18:02:00")
                },
                new TransactionDto
                {
                    ReferenceNumber = "ReferenceNumber3",
                    TotalPrice = 103,
                    Date = DateTime.Parse("06/06/20 18:03:00")
                }
            };

            transactions = new List<Transaction>
            {
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber1",
                    TotalPrice = 101,
                    Date = DateTime.Parse("06/06/20 18:01:00")
                },
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber2",
                    TotalPrice = 102,
                    Date = DateTime.Parse("06/06/20 18:02:00")
                },
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber3",
                    TotalPrice = 103,
                    Date = DateTime.Parse("06/06/20 18:03:00")
                }
            };

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        It.IsAny<Uri>(), It.IsAny<GetTransactionsRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(transactionsDto)
                    .ThrowsAsync(new System.Net.WebException());

            transactionService = new TransactionService(httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Arrange
            IHttpService httpService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TransactionService(httpService));
        }

        [Fact]
        public async Task GetTransactions_CheckTransactions_ShouldBeEqual()
        {
            // Arrange
            var transactionEqualityComparer = new TransactionEqualityComparer();

            // Act
            var actualTransactions = await transactionService.GetTransactionsAsync(email);

            // Assert
            Assert.Equal(transactions, actualTransactions, transactionEqualityComparer);
        }

        [Fact]
        public async Task GetTransactions_CheckTransactions_ShouldThrowException()
        {
            // Act
            await transactionService.GetTransactionsAsync(email);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => transactionService.GetTransactionsAsync(email));
        }
    }
}