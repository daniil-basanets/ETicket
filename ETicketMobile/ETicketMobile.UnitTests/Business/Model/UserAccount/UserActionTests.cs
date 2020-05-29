using ETicketMobile.Business.Model.UserAccount;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.UserAccount
{
    public class UserActionTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var name = "Name";
            var view = "View";

            // Act
            var userAction = new UserAction
            {
                Name = name,
                View = view
            };

            // Assert
            Assert.Equal(name, userAction.Name);
            Assert.Equal(view, userAction.View);
        }
    }
}