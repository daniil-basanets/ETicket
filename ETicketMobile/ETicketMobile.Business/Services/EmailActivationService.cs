using System;
using System.Threading.Tasks;
using ETicketMobile.Business.Services.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;
using Java.Net;

namespace ETicketMobile.Business.Services
{
    public class EmailActivationService : IEmailActivationService
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public EmailActivationService(IHttpService httpService)
        {
            this.httpService = httpService
                ?? throw new ArgumentNullException(nameof(httpService));
        }

        public async Task<bool> ActivateEmailAsync(string email, string code)
        {
            var activateEmailRequest = new ConfirmEmailRequestDto
            {
                Email = email,
                ActivationCode = code
            };

            try
            {
                var response = await httpService.PostAsync<ConfirmEmailRequestDto, ConfirmEmailResponseDto>(
                        AuthorizeEndpoint.CheckCode, activateEmailRequest);

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

        public async Task RequestActivationCodeAsync(string email)
        {
            try
            {
                await httpService.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
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