using System;
using System.Collections.Generic;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.UnitTests.Comparers;
using ETicketMobile.ViewModels.UserAccount;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.UserAccount
{
    public class UserTransactionsViewModelTests
    {
        #region Fields

        private readonly UserTransactionsViewModel userTransactionsViewModel;

        private readonly Mock<ITransactionService> transactionServiceMock;
        private readonly Mock<IPageDialogService> dialogServiceMock;

        private readonly IEnumerable<Transaction> transactions;

        private readonly INavigationParameters navigationParameters;

        private readonly string email;

        #endregion

        public UserTransactionsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();

            email = "email";
            navigationParameters = new NavigationParameters { { email, "email" } };

            transactions = new List<Transaction>
            {
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber1",
                    TotalPrice = 101,
                    Date = DateTime.Parse("02/06/20 17:00:00")
                },
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber2",
                    TotalPrice = 102,
                    Date = DateTime.Parse("02/06/20 18:00:00")
                },
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber3",
                    TotalPrice = 103,
                    Date = DateTime.Parse("02/06/20 19:00:00")
                }
            };

            transactionServiceMock = new Mock<ITransactionService>();
            transactionServiceMock
                    .Setup(ts => ts.GetTransactionsAsync(email))
                    .ReturnsAsync(transactions);

            userTransactionsViewModel = new UserTransactionsViewModel(transactionServiceMock.Object, null, dialogServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableTransactionService_ShouldThrowException()
        {
            // Arrange
            ITransactionService transactionService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(transactionService, null, dialogServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Arrange
            IPageDialogService dialogService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(transactionServiceMock.Object, null, dialogService));
        }

        [Fact]
        public void OnNavigatedTo_CheckTransactions_ShouldBeEqual()
        {
            // Arrange
            var transactionEqualityComparer = new TransactionEqualityComparer();

            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(transactions, userTransactionsViewModel.Transactions, transactionEqualityComparer);
        }
    }
}