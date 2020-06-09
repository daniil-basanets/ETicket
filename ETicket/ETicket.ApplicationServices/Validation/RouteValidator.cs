using ETicket.ApplicationServices.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.ApplicationServices.Validation
{
    public class RouteValidator : AbstractValidator<RouteDto>
    {
        public RouteValidator()
        {
            RuleFor(t => t.Number)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("{PropertyName} is empty")
                 .Length(1, 25).WithMessage("Length {TotalLength} of {PropertyName} is invalid");

            RuleFor(t => t.FirstStationName)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(t => t.LastStationName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
    }
}
