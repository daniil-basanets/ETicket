using System;
using System.Collections.Generic;
using System.Text;
using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class StationValidator : AbstractValidator<StationDto>
    {
        public StationValidator()
        {
            RuleFor(t => t.Name)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("{PropertyName} is empty")
                 .Length(1, 25).WithMessage("Length {TotalLength} of {PropertyName} is invalid");

            RuleFor(t => t.Latitude)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(t => t.Longitude)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}
