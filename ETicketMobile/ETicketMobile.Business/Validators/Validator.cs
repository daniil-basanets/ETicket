using System.Linq;
using Android.Util;

namespace ETicketMobile.Business.Validators
{
    public static class Validator
    {
        #region Constants

        private const int EmailMaxLength = 50;

        private const int PasswordMinLength = 8;
        private const int PasswordMaxLength = 100;

        private const int CardNumberLength = 16;
        private const int ExpirationDateLength = 5;
        private const int CVV2Length = 3;

        private const int NameMinLength = 2;
        private const int NameMaxLength = 25;

        private const int PhoneMaxLength = 13;

        #endregion

        public static bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        public static bool HasEmailCorrectLength(string email)
        {
            return email.Length <= EmailMaxLength;
        }

        public static bool IsPasswordShort(string password)
        {
            return password.Length < PasswordMinLength;
        }

        public static bool IsPasswordLong(string password)
        {
            return password.Length > PasswordMaxLength;
        }

        public static bool IsPasswordWeak(string password)
        {
            return password.All(ch => char.IsDigit(ch));
        }

        public static bool PasswordsMatched(string password, string confirmPassword)
        {
            return string.Equals(password, confirmPassword);
        }

        public static bool HasCardNumberCorrectLength(string cardNumber)
        {
            return cardNumber.Length == CardNumberLength;
        }

        public static bool HasExpirationDateCorrectLength(string expirationDate)
        {
            return expirationDate.Length == ExpirationDateLength;
        }

        public static bool HasCVV2CorrectLength(string cvv2)
        {
            return cvv2.Length == CVV2Length;
        }

        public static bool IsNameValid(string name)
        {
            name ??= string.Empty;

            return name.Length >= NameMinLength && name.Length <= NameMaxLength;
        }

        public static bool HasPhoneCorrectLength(string phone)
        {
            return phone.Length == PhoneMaxLength;
        }

        public static bool TicketChoosed(int count)
        {
            return count != 0;
        }

        public static bool AreaChoosed(int count)
        {
            return count != 0;
        }
    }
}