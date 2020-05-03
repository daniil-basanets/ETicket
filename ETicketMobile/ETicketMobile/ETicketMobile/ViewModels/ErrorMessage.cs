namespace ETicketMobile.ViewModels
{
    public static class ErrorMessage
    {
        #region Email

        private const int EmailMaxLength = 50;

        public static string EmailEmpty = "Enter an email";

        public static string EmailInvalid = "Choose an email address";

        public static string EmailCorrect = "Are you sure you entered your email correctly?";

        public static string EmailCorrectLong = $"Sorry, your username must be fewer than {EmailMaxLength} characters long.";

        public static string EmailTaken = "That username is taken. Try another.";

        public static string EmailWrong = "User with that username doesn't exists.";

        #region Confirm Email

        public static string ConfirmEmailEmpty = "Enter activation code";

        public static string ConfirmEmailWrong = "Wrong activation code";

        #endregion

        #endregion

        #region Password

        private const int PasswordMinLength = 8;
        private const int PasswordMaxLength = 100;

        public static string PasswordStandartFormat = $"Use {PasswordMinLength} or more characters " +
                                                      "with a mix of letters, numbers & symbols";

        public static string PasswordEmpty = "Enter a password";

        public static string PasswordShort = $"Use {PasswordMinLength} characters or more for your password";

        public static string PasswordLong = $"Use {PasswordMaxLength} characters or fewer for your password";

        public static string PasswordStrong = $"Please, choose a stronger password. Try a mix of letters, numbers, symbols.";

        public static string PasswordsMatch = "Please, make sure your passwords match";

        #endregion

        #region First Name

        public static string FirstNameEmpty = "Enter first name";

        public static string FirstNameValid = "Are you sure you entered your first name correctly?";

        #endregion

        #region Last Name

        public static string LastNameEmpty = "Enter last name";

        public static string LastNameValid = "Are you sure you entered your last name correctly?";

        #endregion

        #region Phone

        public static string PhoneFormat = "Phone number must be in format +380 (XX)-XXX-XX-XX";

        #endregion
    }
}