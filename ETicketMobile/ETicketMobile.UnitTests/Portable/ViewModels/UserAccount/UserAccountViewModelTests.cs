using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.UnitTests.Portable.Comparer;
using ETicketMobile.ViewModels.UserAccount;
using ETicketMobile.Views.Tickets;
using ETicketMobile.Views.UserAccount;
using ETicketMobile.Views.UserActions;
using Moq;
using Prism.Navigation;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.UserAccount
{
    public class UserAccountViewModelTests
    {
        #region Fields

        private readonly UserAccountViewModel userAccountViewModel;

        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly UserActionEqualityComparer userActionEqualityComparer;

        private readonly IEnumerable<UserAction> userActions;

        #endregion

        public UserAccountViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            navigationServiceMock = new Mock<INavigationService>();

            userAccountViewModel = new UserAccountViewModel(navigationServiceMock.Object);

            userActionEqualityComparer = new UserActionEqualityComparer();

            userActions = new List<UserAction>
            {
                new UserAction { Name = "Buy Ticket", View = nameof(TicketsView) },
                new UserAction { Name = "Transactions History", View = nameof(UserTransactionsView) },
                new UserAction { Name = "My Tickets", View = nameof(MyTicketsView) }
            };
        }

        [Fact]
        public void Ctor()
        {
            // Act
            var exception = Record.Exception(() => new UserAccountViewModel(null));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void OnAppearing()
        {
            // Act
            userAccountViewModel.OnAppearing();

            // Assert
            Assert.Equal(userActions, userAccountViewModel.UserActions, userActionEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_EmptyEmail()
        {
            // Arrange
            var navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.GetValue<string>(It.IsAny<string>()));

            // Act
            userAccountViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Assert
            navigationParametersMock.Verify(np => np.GetValue<string>("email"), Times.Once);
        }
    }
}