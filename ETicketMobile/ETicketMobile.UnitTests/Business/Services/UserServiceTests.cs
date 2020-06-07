using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Exceptions;
using ETicketMobile.Business.Services;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class UserServiceTests
    {
        #region Fields

        private readonly IUserService userService;

        private readonly Mock<IHttpService> httpServiceMock;

        private readonly CreateNewPasswordResponseDto createNewPasswordResponseDto;

        private readonly UserSignUpRequestDto userSignUpRequestDto;
        private readonly UserSignUpResponseDto userSignUpResponseDto;

        private readonly string email;
        private readonly string password;

        #endregion

        public UserServiceTests()
        {
            email = "email";
            password = "password";

            createNewPasswordResponseDto = new CreateNewPasswordResponseDto();

            userSignUpRequestDto = new UserSignUpRequestDto
            {
                Email = email,
                Password = password,
                FirstName = "firstName",
                LastName = "lastName",
                Phone = "+380679953365",
                DateOfBirth = DateTime.Parse("12/02/1997 12:30:01")
            };

            userSignUpResponseDto = new UserSignUpResponseDto();

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<CreateNewPasswordRequestDto, CreateNewPasswordResponseDto>(
                        It.IsAny<Uri>(), It.IsAny<CreateNewPasswordRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(createNewPasswordResponseDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<UserSignUpRequestDto, UserSignUpResponseDto>(
                        It.IsAny<Uri>(), It.IsAny<UserSignUpRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(userSignUpResponseDto)
                    .ThrowsAsync(new System.Net.WebException());

            userService = new UserService(httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Arrange
            IHttpService httpService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new UserService(httpService));
        }

        [Fact]
        public async Task TryRequestChangePassword_ReturnsTrue()
        {
            // Arrange
            createNewPasswordResponseDto.Succeeded = true;

            // Act
            var actualResult = await userService.RequestChangePasswordAsync(email, password);

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public async Task TryRequestChangePassword_ReturnsFalse()
        {
            // Arrange
            createNewPasswordResponseDto.Succeeded = false;

            // Act
            var actualResult = await userService.RequestChangePasswordAsync(email, password);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public async Task TryRequestChangePassword_ShouldThrowException()
        {
            // Act
            await userService.RequestChangePasswordAsync(email, password);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => userService.RequestChangePasswordAsync(email, password));
        }

        [Fact]
        public async Task TryCreateNewUser_ReturnsTrue()
        {
            // Arrange
            userSignUpResponseDto.Succeeded = true;

            // Act
            var actualResult = await userService.CreateNewUserAsync(userSignUpRequestDto);

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public async Task TryCreateNewUser_ReturnsFalse()
        {
            // Arrange
            userSignUpResponseDto.Succeeded = false;

            // Act
            var actualResult = await userService.CreateNewUserAsync(userSignUpRequestDto);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public async Task TryCreateNewUser_ShouldThrowException()
        {
            // Act
            await userService.CreateNewUserAsync(userSignUpRequestDto);

            // Assert
            await Assert.ThrowsAsync<WebException>(() => userService.CreateNewUserAsync(userSignUpRequestDto));
        }
    }
}