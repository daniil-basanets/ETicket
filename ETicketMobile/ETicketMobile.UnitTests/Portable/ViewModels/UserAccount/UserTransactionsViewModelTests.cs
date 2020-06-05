using System;
using System.Collections.Generic;
using System.Net;
using ETicketMobile.Business.Model.Transactions;
using ETicketMobile.UnitTests.Portable.Comparer;
using ETicketMobile.ViewModels.UserAccount;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
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

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        private readonly IEnumerable<TransactionDto> transactionsDto;
        private readonly IEnumerable<Transaction> transactions;

        private readonly INavigationParameters navigationParameters;

        #endregion

        public UserTransactionsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();
            httpServiceMock = new Mock<IHttpService>();

            var email = "email";
            navigationParameters = new NavigationParameters { { email, "email" } };

            transactionsDto = new List<TransactionDto>
            {
                new TransactionDto
                {
                    ReferenceNumber = "ReferenceNumber",
                    TotalPrice = 100,
                    Date = DateTime.Parse("02/06/20 17:00:00")
                }
            };

            transactions = new List<Transaction>
            {
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber",
                    TotalPrice = 100,
                    Date = DateTime.Parse("02/06/20 17:00:00")
                }
            };

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        It.IsAny<Uri>(), It.IsAny<GetTransactionsRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(transactionsDto)
                    .ThrowsAsync(new WebException());

            userTransactionsViewModel = new UserTransactionsViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableDialogService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void OnNavigatedTo_CompareTransactions_ShouldBeEqual()
        {
            // Arrange
            var transactionEqualityComparer = new TransactionEqualityComparer();

            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(transactions, userTransactionsViewModel.Transactions, transactionEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_CheckThrowWebException()
        {
            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.ThrowsAsync<WebException>(
                () => httpServiceMock.Object.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>> (null, null, null));
        }
    }
}