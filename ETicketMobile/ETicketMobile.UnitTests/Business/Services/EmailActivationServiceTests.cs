using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Services;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Moq;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Services
{
    public class EmailActivationServiceTests
    {
        #region Fields

        private readonly IEmailActivationService emailActivationService;

        private readonly Mock<IHttpService> httpServiceMock;

        private readonly ConfirmEmailResponseDto confirmEmailResponseDto;

        private readonly string email;
        private readonly string code;

        #endregion

        public EmailActivationServiceTests()
        {
            email = "email";
            code = "code";
            
            confirmEmailResponseDto = new ConfirmEmailResponseDto();

            httpServiceMock = new Mock<IHttpService>();
            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<ConfirmEmailRequestDto, ConfirmEmailResponseDto>(
                        It.IsAny<Uri>(), It.IsAny<ConfirmEmailRequestDto>(), It.IsAny<string>()))
                    .ReturnsAsync(confirmEmailResponseDto)
                    .ThrowsAsync(new System.Net.WebException());

            httpServiceMock
                    .SetupSequence(hs => hs.PostAsync<string, string>(
                        It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(string.Empty)
                    .ThrowsAsync(new System.Net.WebException());

            emailActivationService = new EmailActivationService(httpServiceMock.Object);
        }

        [Fact]
        public void CheckConstructorWithParameters_CheckNullableHttpService_ShouldThrowException()
        {
            // Arrange
            IHttpService httpService = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => new EmailActivationService(httpService));
        }

        [Fact]
        public async Task TryActivateEmail_ReturnsTrue()
        {
            // Arrange
            confirmEmailResponseDto.Succeeded = true;

            // Act
            var actualResult = await emailActivationService.ActivateEmailAsync(email, code);

            // Assert
            Assert.True(actualResult);
        }

        [Fact]
        public async Task TryActivateEmail_ReturnsFalse()
        {
            // Arrange
            confirmEmailResponseDto.Succeeded = false;

            // Act
            var actualResult = await emailActivationService.ActivateEmailAsync(email, code);

            // Assert
            Assert.False(actualResult);
        }

        [Fact]
        public async Task VerifyRequestActivationCode()
        {
            // Act
            await emailActivationService.RequestActivationCodeAsync(email);

            // Assert
            httpServiceMock.Verify(hs => hs.PostAsync<string, string>(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}