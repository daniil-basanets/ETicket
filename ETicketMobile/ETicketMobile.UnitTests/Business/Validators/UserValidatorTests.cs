using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Validators;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Validators
{
    public class UserValidatorTests
    {
        #region Fields

        private readonly Mock<IHttpService> httpServiceMock;

        private readonly UserValidator userValidator;
        private readonly SignUpResponseDto signUpResponseDto;

        private readonly string email;

        #endregion

        public UserValidatorTests()
        {
            email = "email";

            signUpResponseDto = new SignUpResponseDto();

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                .SetupSequence(hs => hs.PostAsync<SignUpRequestDto, SignUpResponseDto>(
                    It.IsAny<Uri>(), It.IsAny<SignUpRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(signUpResponseDto)
                .ThrowsAsync(new System.Net.WebException());

            userValidator = new UserValidator(httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new UserValidator(null));
        }

        [Fact]
        public async Task CheckUserExists_ReturnsTrue()
        {
            // Arrange
            signUpResponseDto.Succeeded = true;

            // Act
            var userExists = await userValidator.UserExistsAsync(email);

            // Assert
            Assert.True(userExists);
        }

        [Fact]
        public async Task CheckUserExists_ReturnsFalse()
        {
            // Arrange
            signUpResponseDto.Succeeded = false;

            // Act
            var userExists = await userValidator.UserExistsAsync(email);

            // Assert
            Assert.False(userExists);
        }

        [Fact]
        public async Task GetToken_ShouldThrowException()
        {
            // Act
            await userValidator.UserExistsAsync(email);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => userValidator.UserExistsAsync(email));
        }
    }
}