using ETicketMobile.UserInterface.Behaviors;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.Behaviors
{
    public class EntryMaskBehaviorTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var entryMaskBehavior = new EntryMaskBehavior();

            var mask = "mask";

            // Act
            entryMaskBehavior.Mask = mask;

            // Assert
            Assert.Equal(mask, entryMaskBehavior.Mask);
        }
    }
}