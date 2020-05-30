using System.Threading.Tasks;
using ETicketMobile.Business.Validators.Interfaces;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network.Endpoints;
using ETicketMobile.WebAccess.Network.WebServices.Interfaces;

namespace ETicketMobile.Business.Validators
{
    public class UserValidator : IUserValidator
    {
        #region Fields

        private readonly IHttpService httpService;

        #endregion

        public UserValidator(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var signUpRequestDto = new SignUpRequestDto { Email = email };

            var isUserExists = await httpService.PostAsync<SignUpRequestDto, SignUpResponseDto>(
                AuthorizeEndpoint.CheckEmail, signUpRequestDto);

            return isUserExists.Succeeded;
        }
    }
}