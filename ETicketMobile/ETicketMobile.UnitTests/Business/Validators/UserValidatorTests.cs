using System;
using System.Threading.Tasks;
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

        #endregion

        public UserValidatorTests()
        {
            httpServiceMock = new Mock<IHttpService>();
        }

        [Fact]
        public void Ctor_Positive()
        {
            // Act
            var exception = Record.Exception(() => new UserValidator(httpServiceMock.Object));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Ctor_Negative()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new UserValidator(null));
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsTrue()
        {
            // Arrange
            var signUpResponseDto = new SignUpResponseDto { Succeeded = true };

            httpServiceMock
                .Setup(hs => hs.PostAsync<SignUpRequestDto, SignUpResponseDto>(
                    It.IsAny<Uri>(), It.IsAny<SignUpRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(signUpResponseDto);

            var userValidator = new UserValidator(httpServiceMock.Object);

            // Act
            var userExists = await userValidator.UserExistsAsync("email");

            // Assert
            Assert.True(userExists);
        }

        [Fact]
        public async Task UserExistsAsync_ReturnsFalse()
        {
            // Arrange
            var signUpResponseDto = new SignUpResponseDto { Succeeded = false };

            httpServiceMock
                .Setup(hs => hs.PostAsync<SignUpRequestDto, SignUpResponseDto>(
                    It.IsAny<Uri>(), It.IsAny<SignUpRequestDto>(), It.IsAny<string>()))
                .ReturnsAsync(signUpResponseDto);

            var userValidator = new UserValidator(httpServiceMock.Object);

            // Act
            var userExists = await userValidator.UserExistsAsync("email");

            // Assert
            Assert.False(userExists);
        }
    }
}