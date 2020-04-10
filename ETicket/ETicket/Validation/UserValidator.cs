using System;
using System.Linq;
using System.Text.RegularExpressions;
using DBContextLibrary.Domain.Entities;
using FluentValidation;

namespace ETicket.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght ({TotalLength}) of {PropertyName} Invalid")
                .Must(BeAValidName).WithMessage("{PropertyName} Contains invalid characters");
            
            RuleFor(u => u.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(2, 50).WithMessage("Lenght ({TotalLength}) of {PropertyName} Invalid")
                .Must(BeAValidName).WithMessage("{PropertyName} Contains invalid characters");

            RuleFor(u => u.DateOfBirth)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Must(BeAValidAge).WithMessage("Invalid {PropertyName}");

            RuleFor(u => u.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Must(BeAValidPhoneNumber).WithMessage("Invalid {PropertyName}");
            
            RuleFor(u=>u.Role)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().When(u=>u.RoleId != null)
                .SetValidator(new RoleValidator());

            RuleFor(u => u.Privilege)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().When(u => u.PrivilegeId != null && u.RoleId != null)
                .SetValidator(new PrivilegeValidator());

            RuleFor(t => t.Document)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().When(t=>t.DocumentId != null)
                .SetValidator(new DocumentValidator());

            RuleFor(t => t.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
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
            var currentYear = DateTime.Now.Year;
            var dobYear = date.Year;

            return dobYear <= currentYear && dobYear > (currentYear - 120);
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