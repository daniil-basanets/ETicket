using System;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.DTO
{
    public class UserSignUpRequestDtoTests
    {
        [Fact]
        public void CheckInstanceProperties()
        {
            // Arrange
            var email = "Email";
            var password = "Password";
            var firstName = "FirstName";
            var lastName = "LastName";
            var phone = "Phone";
            var dateOfBirth = DateTime.Now;

            // Act
            var userSignUpRequestDto = new UserSignUpRequestDto
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                DateOfBirth = dateOfBirth
            };

            // Assert
            Assert.Equal(email, userSignUpRequestDto.Email);
            Assert.Equal(password, userSignUpRequestDto.Password);
            Assert.Equal(firstName, userSignUpRequestDto.FirstName);
            Assert.Equal(lastName, userSignUpRequestDto.LastName);
            Assert.Equal(phone, userSignUpRequestDto.Phone);
            Assert.Equal(dateOfBirth, userSignUpRequestDto.DateOfBirth);
        }
    }
}