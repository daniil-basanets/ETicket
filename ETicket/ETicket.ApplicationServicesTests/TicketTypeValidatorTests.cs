using ETicket.ApplicationServices.DTOs;
using ETicket.WebAPI.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace ETicket.ApplicationServicesTests
{
    public class TicketTypeValidatorTests
    {
        private readonly TicketTypeValidator ticketTypeValidator;
        private readonly TicketTypeDto ticketTypeDto;

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
        [InlineData((string) null)]
        [InlineData("q")]
        [InlineData("#")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        [InlineData("                                                        ")]
        [InlineData("  f   ")]
        [InlineData(" \r \t \n" )]
        //[InlineData(0)]
        public void TicketType_TypeName_ShouldFail(string typeName)
        {
            ticketTypeDto.TypeName = typeName;
            
            var isValid = ticketTypeValidator.Validate(ticketTypeDto).IsValid.Equals(true);
            
            Assert.True(isValid);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData((string) null)]
        [InlineData("q")]
        [InlineData("#")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        [InlineData("                                                        ")]
        [InlineData("  f   ")]
        public void TicketType_TypeName_ShouldHaveError(string typeName)
        {
            ticketTypeValidator.ShouldHaveValidationErrorFor(ticketType => ticketType.TypeName, typeName);
        }
    }
}