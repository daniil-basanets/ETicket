using System.Globalization;
using ETicketMobile.Business.Model.UserAccount;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.UserAccount
{
    public class CultureHelperTests
    {
        [Fact]
        public void GetCulture_Positive()
        {
            // Arrange
            var localization = new ETicketMobile.Data.Entities.Localization { Culture = "ru-RU" };
            var expectedCulture = new CultureInfo(localization.Culture);

            // Act
            var actualCulture = CultureHelper.GetCulture(localization);

            // Assert
            Assert.Equal(expectedCulture, actualCulture);
        }

        [Fact]
        public void GetCulture_Negative()
        {
            // Arrange
            var expectedCulture = CultureInfo.CurrentCulture;

            // Act
            var actualCulture = CultureHelper.GetCulture(null);

            // Assert
            Assert.Equal(expectedCulture, actualCulture);
        }
    }
}