using ETicketMobile.Data.Entities;
using Xunit;

namespace ETicketMobile.UnitTests.Data.Entities
{
    public class SettingsTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var id = 0;
            var name = "Name";
            var value = "Value";

            // Act
            var setting = new Setting
            {
                Id = id,
                Name = name,
                Value = value
            };

            // Assert
            Assert.Equal(id, setting.Id);
            Assert.Equal(name, setting.Name);
            Assert.Equal(value, setting.Value);
        }
    }
}