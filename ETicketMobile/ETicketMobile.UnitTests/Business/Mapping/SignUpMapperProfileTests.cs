using ETicketMobile.Business.Mapping;
using ETicketMobile.Business.Model.Registration;
using ETicketMobile.WebAccess.DTO;
using Xunit;

namespace ETicketMobile.UnitTests.Business.Mapping
{
    public class SignUpMapperProfileTests
    {
        [Fact]
        public void MapSignUpRequestDtoToSignUp()
        {
            // Arrange
            var signUpRequestDto = new SignUpRequestDto { Email = "email" };

            // Act
            var signUp = AutoMapperConfiguration.Mapper.Map<SignUpRequestDto, SignUp>(signUpRequestDto);

            // Assert
            Assert.Equal(signUpRequestDto.Email, signUp.Email);
        }

        [Fact]
        public void MapSignUpToSignUpRequestDto()
        {
            // Arrange
            var signUp = new SignUp { Email = "email" };

            // Act
            var signUpRequestDto = AutoMapperConfiguration.Mapper.Map<SignUp, SignUpRequestDto>(signUp);

            // Assert
            Assert.Equal(signUp.Email, signUpRequestDto.Email);
        }

        [Fact]
        public void MapSignUpResponseDtoToSignUp()
        {
            // Arrange
            var signUpResponseDto = new SignUpResponseDto { Succeeded = true };

            // Act
            var signUp = AutoMapperConfiguration.Mapper.Map<SignUpResponseDto, SignUp>(signUpResponseDto);

            // Assert
            Assert.Equal(signUpResponseDto.Succeeded, signUp.Succeeded);
        }

        [Fact]
        public void MapSignUpToSignUpResponseDto()
        {
            // Arrange
            var signUp = new SignUp { Succeeded = true };

            // Act
            var signUpResponseDto = AutoMapperConfiguration.Mapper.Map<SignUp, SignUpResponseDto>(signUp);

            // Assert
            Assert.Equal(signUp.Succeeded, signUpResponseDto.Succeeded);
        }
    }
}