using ETicketMobile.Business.Model.Registration;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Registration
{
    public class ExpirationDateDescriptorTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var expirationMonth = "06";
            var expirationYear = "20";

            // Act
            var expirationDateDescriptor = new ExpirationDateDescriptor
            {
                ExpirationMonth = expirationMonth,
                ExpirationYear = expirationYear
            };

            // Assert
            Assert.Equal(expirationMonth, expirationDateDescriptor.ExpirationMonth);
            Assert.Equal(expirationYear, expirationDateDescriptor.ExpirationYear);
        }
    }
}