using ETicketMobile.Business.Model.Registration;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Registration
{
    public class SignUpTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var email = "email";
            var succeeded = true;

            // Act
            var signUp = new SignUp
            {
                Email = email,
                Succeeded = succeeded
            };

            // Assert
            Assert.Equal(email, signUp.Email);
            Assert.Equal(succeeded, signUp.Succeeded);
        }
    }
}