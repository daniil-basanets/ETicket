using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Transaction;
using ETicket.ApplicationServicesTests.Comparers;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class TransactionHistoryServiceTests
    {
        #region Private Members

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly TransactionHistoryDtoEqualityComparer transactionHistoryDtoEqualityComparer;

        private readonly IEnumerable<TransactionHistoryDto> transactionsDto;
        private readonly IEnumerable<TransactionHistory> transactions;
        private readonly IEnumerable<Ticket> tickets;

        private readonly TransactionService transactionService;

        private readonly TransactionHistoryDto transactionDto;
        private readonly TransactionHistory transaction;

        private readonly Guid transactionEmptyId;
        private readonly Guid transactionId;
        private readonly Guid userId;

        #endregion

        public TransactionHistoryServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

            transactionHistoryDtoEqualityComparer = new TransactionHistoryDtoEqualityComparer();

            transactions = new List<TransactionHistory>
            {
                new TransactionHistory
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber1",
                    TotalPrice = 101,
                    Date = DateTime.Parse("03/06/20 15:00:00")
                },
                new TransactionHistory
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber2",
                    TotalPrice = 102,
                    Date = DateTime.Parse("03/06/20 16:00:00")
                },
                new TransactionHistory
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber3",
                    TotalPrice = 103,
                    Date = DateTime.Parse("03/06/20 17:00:00")
                }
            };

            transactionsDto = new List<TransactionHistoryDto>
            {
                new TransactionHistoryDto
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber1",
                    TotalPrice = 101,
                    Date = DateTime.Parse("03/06/20 15:00:00")
                },
                new TransactionHistoryDto
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber2",
                    TotalPrice = 102,
                    Date = DateTime.Parse("03/06/20 16:00:00")
                },
                new TransactionHistoryDto
                {
                    Id = Guid.Empty,
                    ReferenceNumber = "ReferenceNumber3",
                    TotalPrice = 103,
                    Date = DateTime.Parse("03/06/20 17:00:00")
                }
            };

            transactionDto = new TransactionHistoryDto
            {
                Id = transactionEmptyId,
                ReferenceNumber = "ReferenceNumber",
                TotalPrice = 100,
                Date = DateTime.Parse("03/06/20 15:00:00")
            };

            transaction = new TransactionHistory
            {
                Id = transactionEmptyId,
                ReferenceNumber = "ReferenceNumber",
                TotalPrice = 100,
                Date = DateTime.Parse("03/06/20 15:00:00")
            };

            transactionEmptyId = Guid.Empty;
            transactionId = Guid.NewGuid();
            userId = Guid.Parse("A2BA5D10-C628-4858-B821-D6F0399D01BC");

            tickets = new Ticket[]
            {
                new Ticket
                {
                    UserId = userId,
                    TransactionHistory = transactions.ElementAt(0)
                },
                new Ticket
                {
                    UserId = userId,
                    TransactionHistory = transactions.ElementAt(1)
                },
                new Ticket
                {
                    UserId = userId,
                    TransactionHistory = transactions.ElementAt(2)
                }
            };

            unitOfWorkMock.Setup(uow => uow.TransactionHistory.Create(It.IsAny<TransactionHistory>()));

            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.Get(transactionId))
                    .Returns(transaction);

            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.GetAll())
                    .Returns(transactions.AsQueryable);

            unitOfWorkMock
                .Setup(uow => uow.Tickets.GetAll())
                .Returns(tickets.AsQueryable);

            var id = Guid.Parse("1DE63B31-A62D-4D5A-9BC7-846EB2E2BADE");
            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.Get(id))
                    .Returns(() => null);

            transactionService = new TransactionService(unitOfWorkMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableUnitOfWork_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TransactionService(null));
        }

        [Fact]
        public void Create_Transaction()
        {
            // Act
            transactionService.AddTransaction(transactionDto);

            // Assert
            unitOfWorkMock.Verify(uow => uow.TransactionHistory.Create(It.IsAny<TransactionHistory>()), Times.Once);
        }

        [Fact]
        public void Create_Transaction_CheckZeroTotalPrice_ShouldThrowException()
        {
            // Act
            transactionDto.TotalPrice = 0;

            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.AddTransaction(transactionDto));
        }

        [Fact]
        public void Create_Transaction_CheckNullableTransaction_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => transactionService.AddTransaction(null));
        }

        [Fact]
        public void GetTransactions_CompareTransactions_ShouldBeEqual()
        {
            // Act
            var actualTransactions = transactionService.GetTransactions();

            // Assert
            Assert.Equal(transactionsDto, actualTransactions, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionsByUserId_CompareTransactions_ShouldBeEqual()
        {
            // Act
            var actualTransactions = transactionService.GetTransactionsByUserId(userId);

            // Assert
            Assert.Equal(transactionsDto, actualTransactions, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionsByUserId_CheckEmptyUserId_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.GetTransactionsByUserId(transactionEmptyId));
        }

        [Fact]
        public void GetTransactionById_CompareTransactions_ShouldBeEqual()
        {
            // Act
            var actualTransaction = transactionService.GetTransactionById(transactionId);

            // Assert
            Assert.Equal(transactionDto, actualTransaction, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionById_CheckNullableTransaction_ShouldThrowException()
        {
            // Arrange
            var id = Guid.Parse("1DE63B31-A62D-4D5A-9BC7-846EB2E2BADE");

            // Assert
            Assert.Throws<NullReferenceException>(() => transactionService.GetTransactionById(id));
        }

        [Fact]
        public void GetTransactionById_CheckEmptyTransactionId_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.GetTransactionById(transactionEmptyId));
        }
    }
}