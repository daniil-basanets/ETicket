using ETicket.ApplicationServices.DTOs;
using ETicket.WebAPI.Validation;
using Xunit;

namespace ETicket.ApplicationServicesTests
{
    public class TicketTypeValidatorTests
    {
        private readonly TicketTypeValidator ticketTypeValidator;
        private TicketTypeDto ticketTypeDto;

        public TicketTypeValidatorTests()
        {
            ticketTypeValidator = new TicketTypeValidator();
            ticketTypeDto = new TicketTypeDto
            {
                TypeName = "Test",
                IsPersonal = true,
                DurationHours = 12,
                Price = 25M
            };
        }

        [Theory]
        [InlineData("")]
        [InlineData("q")]
        [InlineData("#")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        [InlineData("                                                        ")]
        [InlineData("  f   ")]
        public void TicketType_TypeName_ShouldCorrect(string typeName)
        {
            ticketTypeDto.TypeName = typeName;
            var valid = ticketTypeValidator.Validate(ticketTypeDto).IsValid.Equals(true);

            Assert.False(valid);
        }
    }
}