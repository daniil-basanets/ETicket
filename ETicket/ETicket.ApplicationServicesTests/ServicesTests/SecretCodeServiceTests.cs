using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Moq;
using Xunit;

namespace ETicket.ApplicationServicesTests.ServicesTests
{
    public class SecretCodeServiceTests
    {
        #region Private members

        private readonly SecretCodeService secretCodeService;
        private readonly List<SecretCode> fakeCodes;
        private SecretCode secretCode;

        #endregion

        #region Constructor

        public SecretCodeServiceTests()
        {
            var mockRepository = new Mock<ISecretCodeRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            fakeCodes = new List<SecretCode>
            {
                new SecretCode{Id = 1, Email = "email1@gmail.com", Code = "1111kj"},
                new SecretCode{Id = 2, Email = "email2@gmail.com", Code = "2222kj"},
                new SecretCode{Id = 3, Email = "email3@gmail.com", Code = "3333kj"},
                new SecretCode{Id = 4, Email = "email3@gmail.com", Code = "4444kj"}
            };

            mockRepository.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((code, email) => { return Task.FromResult(fakeCodes.Single(c => c.Email == email && c.Code == code)); });
            mockRepository.Setup(r => r.Add(It.IsAny<SecretCode>()))
                .Callback<SecretCode>(s => fakeCodes.Add(secretCode = s));
            mockRepository.Setup(x => x.RemoveRange(It.IsAny<string>()))
                .Callback<string>((email) => fakeCodes.RemoveAll(secretCode => secretCode.Email == email));
            mockRepository.Setup(y => y.Count(It.IsAny<string>()))
                .Returns<string>(email => fakeCodes.Count(x => x.Email == email));

            mockUnitOfWork.Setup(m => m.SecretCodes).Returns(mockRepository.Object);

            secretCodeService = new SecretCodeService(mockUnitOfWork.Object);
        }

        #endregion

        #region GetCode facts

        [Theory]
        [InlineData("email1@gmail.com", "1111kj")]
        [InlineData("email2@gmail.com", "2222kj")]
        public void Get_CheckCodeInReceivedObject_ShouldBeTheSameAsInFake(string email, string code)
        {
            var expected = fakeCodes.Single(c => c.Email == email && c.Code == code).Code;
            var actual = secretCodeService.Get(code, email).Result.Code;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region CreateCode facts

        [Fact]
        public void Create_Code_ShouldBeNotNull()
        {
            secretCodeService.Add(new SecretCode { Id = 8, Code = "1234uh", Email = "Test@gmail.com" });

            var actual = secretCode;

            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_CheckCodeInNewObject_ShouldBeTheSameAsInFake()
        {
            secretCodeService.Add(new SecretCode { Id = 8, Code = "1234uh", Email = "Test@gmail.com" });

            var expected = "1234uh";
            var actual = secretCode.Code;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_AddNewObject_CountShouldIncrease()
        {
            var expected = fakeCodes.Count + 1;

            secretCodeService.Add(new SecretCode { Id = 8, Code = "1234uh", Email = "Test@gmail.com" });

            var actual = fakeCodes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, "1111ok")]
        [InlineData("", "1111ok")]
        [InlineData("  \n", "1234OK")]
        public void Create_Code_ShouldFailEmailIsEmpty(string email, string code)
        {
            var sc = new SecretCode { Id = 8, Code = code, Email = email };

            var exception = Assert.Throws<ArgumentException>(() => secretCodeService.Add(sc));

            var expectedMessage = "Email is empty";
            Assert.Equal(expectedMessage, exception.Message);
        }

        #endregion

        #region RemoveRange facts

        [Fact]
        public void Delete_code_CountShouldDecrease()
        {
            var expected = fakeCodes.Count - 1;

            secretCodeService.RemoveRange(fakeCodes.First().Email);

            var actual = fakeCodes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("email3@gmail.com")]
        public void RemoveRange_code_CountShouldDecrease(string email)
        {
            var expected = fakeCodes.Count - 2;

            secretCodeService.RemoveRange(email);

            var actual = fakeCodes.Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("dfgbh")]
        [InlineData("sdfghjkl;")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Delete_Code_CountShouldNotDecrease(string email)
        {
            var expected = fakeCodes.Count;

            secretCodeService.RemoveRange(email);

            var actual = fakeCodes.Count;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Count facts

        [Theory]
        [InlineData("email3@gmail.com")]
        public void Count_Code_CountShouldBe2(string email)
        {
            var expected = fakeCodes.Count(x => x.Email == email);

            var actual = secretCodeService.Count(email);

            var expectedCount = 2;
            Assert.Equal(expectedCount, actual);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\n \t    ")]
        [InlineData(null)]
        public void Count_Code_CountShouldBe0(string email)
        {
            var expected = fakeCodes.Count(x => x.Email == email);

            var actual = secretCodeService.Count(email);

            Assert.Equal(0, actual);
        }

        #endregion
    }
}