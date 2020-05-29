using ETicketMobile.Business.Mapping;
using ETicketMobile.Data.Entities;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class LocalizationMapperProfileTests
    {
        [Fact]
        public void MapEntityLocalizationToLocalization()
        {
            // Arrange
            var entityLocalization = new Localization { Culture = "ru-RU" };

            // Act
            var localization = AutoMapperConfiguration.Mapper
                    .Map<Localization, ETicketMobile.Business.Model.UserAccount.Localization>(entityLocalization);

            // Assert
            Assert.Equal(entityLocalization.Culture, localization.Culture);
        }

        [Fact]
        public void MapLocalizationToEntityLocalization()
        {
            // Arrange
            var localization = new ETicketMobile.Business.Model.UserAccount.Localization { Culture = "ru-RU" };

            // Act
            var entityLocalization = AutoMapperConfiguration.Mapper
                    .Map<ETicketMobile.Business.Model.UserAccount.Localization, Localization>(localization);

            // Assert
            Assert.Equal(localization.Culture, entityLocalization.Culture);
        }
    }
}