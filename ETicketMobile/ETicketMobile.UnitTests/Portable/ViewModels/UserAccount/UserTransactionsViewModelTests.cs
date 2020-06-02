using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Mock<INavigationParameters> navigationParametersMock;

        #endregion

        public UserTransactionsViewModelTests()
        {
            dialogServiceMock = new Mock<IPageDialogService>();
            httpServiceMock = new Mock<IHttpService>();

            navigationParametersMock = new Mock<INavigationParameters>();
        }

        [Fact]
        public void CtorWithParameters()
        {
            // Act
            var exception = Record.Exception(() => new UserTransactionsViewModel(null, dialogServiceMock.Object, httpServiceMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CtorWithParameters_NullDialogService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, null, httpServiceMock.Object));
        }

        [Fact]
        public void CtorWithParameters_NullHttpService()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new UserTransactionsViewModel(null, dialogServiceMock.Object, null));
        }

        [Fact]
        public void OnNavigatedTo()
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
            userTransactionsViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Assert
            Assert.Equal(transactions, userTransactionsViewModel.Transactions, transactionEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_WebException()
        {
            // Arrange
            var webException = new WebException();

            httpServiceMock
                    .Setup(hs => hs.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>>(
                        It.IsAny<Uri>(), It.IsAny<GetTransactionsRequestDto>(), It.IsAny<string>()))
                    .ThrowsAsync(webException);

            var userTransactionsViewModel = new UserTransactionsViewModel(null, dialogServiceMock.Object, httpServiceMock.Object);

            // Act
            userTransactionsViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Assert
            Assert.ThrowsAsync<WebException>(() => httpServiceMock.Object.PostAsync<GetTransactionsRequestDto, IEnumerable<TransactionDto>> (null, null, null));
        }
    }
}