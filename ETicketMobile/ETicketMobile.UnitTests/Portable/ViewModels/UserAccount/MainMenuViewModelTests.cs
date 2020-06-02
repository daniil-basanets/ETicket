using ETicketMobile.ViewModels.UserAccount;
using Xunit;

namespace ETicketMobile.UnitTests.Portable.ViewModels.UserAccount
{
    public class MainMenuViewModelTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var exception = Record.Exception(() => new MainMenuViewModel());

            // Assert
            Assert.Null(exception);
        }
    }
}