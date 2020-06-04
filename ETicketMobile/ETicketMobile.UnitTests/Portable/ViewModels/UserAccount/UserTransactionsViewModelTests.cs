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

        private readonly Mock<IPageDialogService> dialogServiceMock;
        private readonly Mock<IHttpService> httpServiceMock;

        private readonly INavigationParameters navigationParameters;

        #endregion

        public UserTransactionsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();
            httpServiceMock = new Mock<IHttpService>();

            var email = "email";
            navigationParameters = new NavigationParameters { { email, "email" } };
        }

        [Fact]
        public void CtorWithParameters_NullDialogService_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullHttpService_ThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void OnNavigatedTo_ReturnsTransactions()
        {
            // Arrange
            var transactionEqualityComparer = new TransactionEqualityComparer();

            var transactionsDto = new List<TransactionDto>
            {
                new TransactionDto
                {
                    ReferenceNumber = "ReferenceNumber",
                    TotalPrice = 100,
                    Date = DateTime.Parse("02/06/20 17:00:00")
                }
            };

            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    ReferenceNumber = "ReferenceNumber",
                    TotalPrice = 100,
                    Date = DateTime.Parse("02/06/20 17:00:00")
                }
            };

            httpServiceMock
                    .Setup(hs => hs.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        It.IsAny<Uri>(), It.IsAny<GetTransactionsRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(transactionsDto);

            var userTransactionsViewModel = new UserTransactionsViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);

            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.Equal(transactions, userTransactionsViewModel.Transactions, transactionEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_ThrowWebException()
        {
            // Arrange
            var webException = new WebException();

            httpServiceMock
                    .Setup(hs => hs.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        It.IsAny<Uri>(), It.IsAny<GetTransactionsRequestDto>(), It.IsAny<string>()))
                    .ThrowsAsync(webException);

            var userTransactionsViewModel = new UserTransactionsViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);

            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParameters);

            // Assert
            Assert.ThrowsAsync<WebException>(() => httpServiceMock.Object.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>> (null, null, null));
        }
    }
}