using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Validators
{
    public class UserValidator : IUserValidator
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public UserValidator(IHttpService httpService)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                var signUpRequestDto = new SignUpRequestDto { Email = email };

                var isUserExists = await httpService.PostAsync<SignUpRequestDto, SignUpResponseDto>(
                    AuthorizeEndpoint.CheckUserExists, signUpRequestDto);

                return isUserExists.Succeeded;
            }
            catch (System.Net.WebException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
            catch (SocketException ex)
            {
                throw new Exceptions.WebException("Server exception", ex);
            }
        }
    }
}