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

        private readonly TransactionHistory transaction;
        private readonly TransactionHistoryDto transactionDto;

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
                Id = Guid.Empty,
                ReferenceNumber = "ReferenceNumber",
                TotalPrice = 100,
                Date = DateTime.Parse("03/06/20 15:00:00")
            };

            transaction = new TransactionHistory
            {
                Id = Guid.Empty,
                ReferenceNumber = "ReferenceNumber",
                TotalPrice = 100,
                Date = DateTime.Parse("03/06/20 15:00:00")
            };

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
        }

        [Fact]
        public void Ctor_NullUnitOfWork_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new TransactionService(null));
        }

        [Fact]
        public void AddTransaction()
        {
            // Arrange
            var transactionServie = new TransactionService(unitOfWorkMock.Object);

            // Act
            transactionServie.AddTransaction(transactionDto);

            // Assert
            unitOfWorkMock.Verify();
        }

        [Fact]
        public void AddTransaction_ZeroTotalPrice_ThrowArgumentException()
        {
            // Arrange
            var transactionService = new TransactionService(unitOfWorkMock.Object);

            // Act
            transactionDto.TotalPrice = 0;

            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.AddTransaction(transactionDto));
        }

        [Fact]
        public void AddTransaction_NullTransaction_ThrowArgumentNullException()
        {
            // Arrange
            var transactionService = new TransactionService(unitOfWorkMock.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(() => transactionService.AddTransaction(null));
        }

        [Fact]
        public void GetTransactions()
        {
            // Arrange
            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.GetAll())
                    .Returns(transactions.AsQueryable);

            var transactionServie = new TransactionService(unitOfWorkMock.Object);

            // Act
            var actualTransactions = transactionServie.GetTransactions();

            // Assert
            Assert.Equal(transactionsDto, actualTransactions, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionsByUserId()
        {
            // Arrange
            unitOfWorkMock
                .Setup(uow => uow.Tickets.GetAll())
                .Returns(tickets.AsQueryable);

            var transactionServie = new TransactionService(unitOfWorkMock.Object);

            // Act
            var actualTransactions = transactionServie.GetTransactionsByUserId(userId);

            // Assert
            Assert.Equal(transactionsDto, actualTransactions, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionsByUserId_EmptyUserId_ThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            var transactionService = new TransactionService(unitOfWorkMock.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.GetTransactionsByUserId(id));
        }

        [Fact]
        public void GetTransactionById()
        {
            // Arrange
            var id = Guid.NewGuid();

            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.Get(id))
                    .Returns(transaction);

            var transactionServie = new TransactionService(unitOfWorkMock.Object);

            // Act
            var actualTransaction = transactionServie.GetTransactionById(id);

            // Assert
            Assert.Equal(transactionDto, actualTransaction, transactionHistoryDtoEqualityComparer);
        }

        [Fact]
        public void GetTransactionById_NullTransaction_ThrowArgumentNullException()
        {
            // Arrange
            var id = Guid.NewGuid();

            unitOfWorkMock
                    .Setup(uow => uow.TransactionHistory.Get(id))
                    .Returns(() => null);

            var transactionService = new TransactionService(unitOfWorkMock.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(() => transactionService.GetTransactionById(id));
        }

        [Fact]
        public void GetTransactionById_EmptyTransactionId_ThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            var transactionService = new TransactionService(unitOfWorkMock.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => transactionService.GetTransactionById(id));
        }
    }
}