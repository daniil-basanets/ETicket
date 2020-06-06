using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Services
{
    public class UserService : IUserService
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public UserService(IHttpService httpService)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<bool> RequestChangePasswordAsync(string email, string password)
        {
            try
            {
                var createNewPasswordDto = new CreateNewPasswordRequestDto
                {
                    Email = email,
                    NewPassword = password
                };

                var response = await httpService.PostAsync<CreateNewPasswordRequestDto, CreateNewPasswordResponseDto>(
                        AuthorizeEndpoint.ResetPassword, createNewPasswordDto);

                return response.Succeeded;
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

        public async Task<bool> CreateNewUserAsync(UserSignUpRequestDto user)
        {
            try
            {
                var response = await httpService
                    .PostAsync<UserSignUpRequestDto, UserSignUpResponseDto>(AuthorizeEndpoint.Registration, user);

                return response.Succeeded;
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