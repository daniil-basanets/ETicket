using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class AreaDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var id = 1;
            var name = "Name";
            var description = "Description";

            // Act
            var areaDto = new AreaDto
            {
                Id = id,
                Name = name,
                Description = description
            };

            // Assert
            Assert.Equal(id, areaDto.Id);
            Assert.Equal(name, areaDto.Name);
            Assert.Equal(description, areaDto.Description);
        }
    }
}