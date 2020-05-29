using ETicketMobile.Business.Model.Tickets;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Tickets
{
    public class AreaTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var id = 1;
            var name = "Name";
            var description = "Description";
            var isChecked = true;

            // Act
            var area = new Area
            {
                Id = id,
                Name = name,
                Description = description,
                IsChecked = isChecked
            };

            // Assert
            Assert.Equal(id, area.Id);
            Assert.Equal(name, area.Name);
            Assert.Equal(description, area.Description);
            Assert.Equal(isChecked, area.IsChecked);
        }
    }
}