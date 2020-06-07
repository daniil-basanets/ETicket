using System;
using System.Linq;
using System.Text.RegularExpressions;
using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50)
                .Must(BeAValidName).WithMessage("{PropertyName} Contains invalid characters");
            
            RuleFor(u => u.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50)
                .Must(BeAValidName).WithMessage("{PropertyName} Contains invalid characters");

            RuleFor(u => u.DateOfBirth)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Must(BeAValidAge).WithMessage("Invalid {PropertyName}");

            RuleFor(u => u.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Must(BeAValidPhoneNumber).WithMessage("Invalid {PropertyName}");
            
            RuleFor(t => t.Email)
                .EmailAddress().WithMessage("Invalid {PropertyName}");
        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");

            return name.All(char.IsLetter);
        }

        private bool BeAValidAge(DateTime date)
        {
            var result = DateTime.Compare(date.Date, DateTime.UtcNow.Date);
    
            return (result < 0) && date.Date.Year > (DateTime.UtcNow.Year - 120);
        }

        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            var cleaned = RemoveNonNumeric(phoneNumber);
            
            return cleaned.Length > 9 && cleaned.Length < 14;
        }
        
        private string RemoveNonNumeric(string phoneNumber)
        {
            return Regex.Replace(phoneNumber, @"[^0-9]+", "");
        }
    }
}