using System;
using ETicketMobile.WebAccess.Network.Configs;
using ETicketMobile.WebAccess.Network.WebServices;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Xunit;

namespace ETicketMobile.UnitTests.WebAccess.Network.WebServices
{
    public class HttpServiceTests
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public HttpServiceTests()
        {
            httpService = new HttpService(ServerConfig.Address);
        }

        [Fact]
        public void CheckContructorWithParameters_CheckNullableUri_ShouldThrowException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HttpService(null));
        }
    }
}