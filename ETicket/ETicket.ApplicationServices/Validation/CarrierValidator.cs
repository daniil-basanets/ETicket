using ETicket.ApplicationServices.DTOs;
using FluentValidation;

namespace ETicket.ApplicationServices.Validation
{
    public class CarrierValidator : AbstractValidator<CarrierDto>
    {
        private const int MaxNameAndAddressLength = 50;
        private const int MinStringLength = 2;
        private const int MaxIBANAddressLength = 30;

        public CarrierValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .Length(MinStringLength, MaxNameAndAddressLength);

            RuleFor(t => t.Address)
                .NotEmpty()
                .Length(MinStringLength, MaxNameAndAddressLength);

            RuleFor(t => t.IBAN)
                .NotEmpty()
                .NotNull()
                .Length(MinStringLength, MaxIBANAddressLength);

            RuleFor(t => t.Phone)
                .NotEmpty()
                .NotNull()
                .Must(IsPhoneNumberValid);
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return false;
            }

            var cleaned = RemoveNonNumeric(phoneNumber);

            return cleaned.Length > 9 && cleaned.Length < 14;
        }

        private string RemoveNonNumeric(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^0-9]+", "");
        }
    }
}
