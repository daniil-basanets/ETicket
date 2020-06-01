using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace ETicket.ApplicationServicesTests.ValidatorsTests
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
                Id = 1,
                DurationHours = 13,
                Coefficient = 15M,
                IsPersonal = true,
                TypeName = "Test"
            };
        }
        
        [Theory]
        [InlineData("")]
        [InlineData((string) null)]
        [InlineData("q")]
        [InlineData("qwertyuioplkjhgfdsazxcvbnmetretertretertertsadasdsadsa")]
        [InlineData("                                                        ")]
        [InlineData("  f   ")]
        [InlineData(" \r \t \n" )]
        public void TicketType_CheckTypeName_ShouldHaveValidationError(string typeName)
        {
            ticketTypeDto.TypeName = typeName;
            var result = ticketTypeValidator.TestValidate(ticketTypeDto);
            
            result.ShouldHaveValidationErrorFor(t => t.TypeName);
        }

        [Fact]
        public void TicketType_CheckDurationHours_ShouldHaveValidationError()
        {
            ticketTypeDto.DurationHours = uint.MinValue;
            var result = ticketTypeValidator.TestValidate(ticketTypeDto);
            
            result.ShouldHaveValidationErrorFor(t => t.DurationHours);
        }
        
        [Fact]
        public void TicketType_CheckCoefficient_ShouldHaveValidationErrorForCoefficient()
        {
            ticketTypeDto.Coefficient = decimal.MinusOne;
            var result = ticketTypeValidator.TestValidate(ticketTypeDto);
            
            result.ShouldHaveValidationErrorFor(t => t.Coefficient);
        }
        
        [Fact]
        public void TicketType_CheckCoefficient_ShouldHaveValidationError()
        {
            ticketTypeDto.Coefficient = decimal.MinValue;
            var result = ticketTypeValidator.TestValidate(ticketTypeDto);
            
            result.ShouldHaveValidationErrorFor(t => t.Coefficient);
        }
    }
}