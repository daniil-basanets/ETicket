using System;
using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class UserMapperProfileTests
    {
        [Fact]
        public void MapUserSignInRequestDtoToUser()
        {
            // Arrange
            var userSignInRequestDto = new UserSignInRequestDto
            {
                Email = "email",
                Password = "password"
            };

            // Act
            var user = AutoMapperConfiguration.Mapper.Map<UserSignInRequestDto, User>(userSignInRequestDto);

            // Assert
            Assert.Equal(userSignInRequestDto.Email, user.Email);
            Assert.Equal(userSignInRequestDto.Password, user.Password);
        }

        [Fact]
        public void MapUserToUserSignInRequestDto()
        {
            // Arrange
            var user = new User
            {
                Email = "email",
                Password = "password"
            };

            // Act
            var userSignInRequestDto = AutoMapperConfiguration.Mapper.Map<User, UserSignInRequestDto>(user);

            // Assert
            Assert.Equal(user.Email, userSignInRequestDto.Email);
            Assert.Equal(user.Password, userSignInRequestDto.Password);
        }

        [Fact]
        public void MapUserSignUpRequestDtoToUser()
        {
            // Arrange
            var userSignUpRequestDto = new UserSignUpRequestDto
            {
                Email = "email",
                Password = "password",
                FirstName = "first name",
                LastName = "last name",
                Phone = "+380679953365",
                DateOfBirth = DateTime.Parse("08/11/1990")
            };

            // Act
            var user = AutoMapperConfiguration.Mapper.Map<UserSignUpRequestDto, User>(userSignUpRequestDto);

            // Assert
            Assert.Equal(userSignUpRequestDto.Email, user.Email);
            Assert.Equal(userSignUpRequestDto.Password, user.Password);
            Assert.Equal(userSignUpRequestDto.FirstName, user.FirstName);
            Assert.Equal(userSignUpRequestDto.LastName, user.LastName);
            Assert.Equal(userSignUpRequestDto.Phone, user.Phone);
            Assert.Equal(userSignUpRequestDto.DateOfBirth, user.DateOfBirth);
        }

        [Fact]
        public void MapUserToUserSignUpRequestDto()
        {
            // Arrange
            var user = new User
            {
                Email = "email",
                Password = "password",
                FirstName = "first name",
                LastName = "last name",
                Phone = "+380679953365",
                DateOfBirth = DateTime.Parse("08/11/1990")
            };

            // Act
            var userSignUpRequestDto = AutoMapperConfiguration.Mapper.Map<User, UserSignUpRequestDto>(user);

            // Assert
            Assert.Equal(userSignUpRequestDto.Email, user.Email);
            Assert.Equal(userSignUpRequestDto.Password, user.Password);
            Assert.Equal(userSignUpRequestDto.FirstName, user.FirstName);
            Assert.Equal(userSignUpRequestDto.LastName, user.LastName);
            Assert.Equal(userSignUpRequestDto.Phone, user.Phone);
            Assert.Equal(userSignUpRequestDto.DateOfBirth, user.DateOfBirth);
        }
    }
}