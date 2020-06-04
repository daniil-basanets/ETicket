using ETicketMobile.UserInterface.Behaviors;
using Xunit;

namespace ETicketMobile.UnitTests.UserInterface.Behaviors
{
    public class EntryMaskBehaviorTests
    {
        #region Fields

        private readonly EntryMaskBehavior entryMaskBehavior;

        #endregion

        public EntryMaskBehaviorTests()
        {
            entryMaskBehavior = new EntryMaskBehavior();
        }

        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var mask = "mask";

            // Act
            entryMaskBehavior.Mask = mask;

            // Assert
            Assert.Equal(mask, entryMaskBehavior.Mask);
        }
    }
}