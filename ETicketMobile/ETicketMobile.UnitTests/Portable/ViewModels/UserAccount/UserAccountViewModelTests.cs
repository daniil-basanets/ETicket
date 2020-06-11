using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.UnitTests.Comparers;
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

        private readonly Mock<INavigationParameters> navigationParametersMock;
        private readonly Mock<INavigationService> navigationServiceMock;

        private readonly UserActionEqualityComparer userActionEqualityComparer;

        private readonly IEnumerable<UserAction> userActions;

        #endregion

        public UserAccountViewModelTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            navigationServiceMock = new Mock<INavigationService>();

            userActionEqualityComparer = new UserActionEqualityComparer();

            userActions = new List<UserAction>
            {
                new UserAction { Name = "Buy Ticket", View = nameof(TicketsView) },
                new UserAction { Name = "Transactions History", View = nameof(UserTransactionsView) },
                new UserAction { Name = "My Tickets", View = nameof(MyTicketsView) }
            };

            navigationParametersMock = new Mock<INavigationParameters>();
            navigationParametersMock.Setup(np => np.GetValue<string>(It.IsAny<string>()));

            userAccountViewModel = new UserAccountViewModel(navigationServiceMock.Object);
        }

        [Fact]
        public void OnAppearing_CheckUserActions_ShouldBeEqual()
        {
            // Act
            userAccountViewModel.OnAppearing();

            // Assert
            Assert.Equal(userActions, userAccountViewModel.UserActions, userActionEqualityComparer);
        }

        [Fact]
        public void OnNavigatedTo_ValidThatEmptyEmail()
        {
            // Act
            userAccountViewModel.OnNavigatedTo(navigationParametersMock.Object);

            // Assert
            navigationParametersMock.Verify();
        }

        [Fact]
        public void OnNavigatedTo_CheckNullableNavigationParameters_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => userAccountViewModel.OnNavigatedTo(null));
        }
    }
}