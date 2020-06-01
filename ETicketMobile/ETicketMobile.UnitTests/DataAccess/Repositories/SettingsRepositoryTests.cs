using ETicketMobile.DataAccess.Repositories;
using Xunit;

namespace ETicketMobile.UnitTests.DataAccess.Repositories
{
    public class SettingsRepositoryTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var exception = Record.Exception(() => new SettingsRepository());

            // Assert
            Assert.Null(exception);
        }
    }
}