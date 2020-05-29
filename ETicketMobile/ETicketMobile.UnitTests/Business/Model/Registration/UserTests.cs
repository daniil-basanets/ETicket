using System;
using ETicketMobile.Business.Model.Registration;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Model.Registration
{
    public class UserTests
    {
        [Fact]
        public void CreateInstance()
        {
            // Arrange
            var email = "Email";
            var firstName = "first name";
            var lastName = "last name";
            var phone = "+380679953365";
            var birthday = DateTime.Now;

            // Act
            var user = new User
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                DateOfBirth = birthday
            };

            // Assert
            Assert.Equal(email, user.Email);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(phone, user.Phone);
            Assert.Equal(birthday, user.DateOfBirth);
        }
    }
}