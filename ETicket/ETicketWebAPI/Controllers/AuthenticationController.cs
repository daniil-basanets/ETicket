using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.WebAPI.Models.Identity;
using ETicket.WebAPI.Models.Identity.Requests;
using ETicket.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Private members
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ETicketDataContext context;
        private IdentityResult identityResult;
        private IdentityUser user;
        private readonly IMailService mailService;
        private readonly IUserService ETUserService;
        private readonly ISecretCodeService codeService;
        #endregion

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ETicketDataContext context, IMailService mailService, IUserService ETUserService, ISecretCodeService codeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.mailService = mailService;
            this.ETUserService = ETUserService;
            this.codeService = codeService;
        }

        // Check if email exists
        // POST: api/authentication/check-user
        [HttpPost("/check-user")]
        public IActionResult CheckEmail([FromBody] RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                bool succeeded = false;
                if (userManager.FindByEmailAsync(request.Email).Result != null)
                {
                    succeeded = true;
                }

                return StatusCode(200, new { succeeded });
            }

            return StatusCode(400, new { ModelState.IsValid });
        }

        // Registration user
        // POST: api/authentication/registration
        [HttpPost("/registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                user = new IdentityUser()
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                identityResult = await userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    UserDto userDto = new UserDto
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Phone = request.Phone,
                        Email = request.Email,
                        DateOfBirth = request.DateOfBirth
                    };
                    ETUserService.CreateUser(userDto);

                    return Ok(new { identityResult.Succeeded });
                }
                else
                {
                    return StatusCode(500, new { identityResult.Succeeded });
                }
            }

            return StatusCode(400, new { ModelState.IsValid });
        }

        // Login user
        // POST: api/authentication/token
        [HttpPost("/token")]
        public async Task<IActionResult> GetToken([FromBody] AuthenticationRequest request)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

                if (signInResult.Succeeded)
                {
                    user = await userManager.FindByNameAsync(request.Email);

                    await userManager.RemoveAuthenticationTokenAsync(user, AuthOptions.ISSUER, "RefreshToken");
                    var refresh_jwtToken = await userManager.GenerateUserTokenAsync(user, AuthOptions.ISSUER, "RefreshToken");
                    await userManager.SetAuthenticationTokenAsync(user, AuthOptions.ISSUER, "RefreshToken", refresh_jwtToken);

                    string access_jwtToken = TokenFactory.GetAccessToken(user);

                    return new JsonResult(new { access_jwtToken, refresh_jwtToken });
                }

                return StatusCode(500, new { signInResult.Succeeded });
            }

            return StatusCode(400, new { ModelState.IsValid });
        }

        // Refresh access_jwtToken
        // POST: api/authentication/refresh-token
        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshUserToken([FromBody] string RefreshToken)
        {
            var token = await context.UserTokens.FirstOrDefaultAsync(refT => refT.Value == RefreshToken);// check if token exists

            if (token != null)
            {
                user = await userManager.FindByIdAsync(token.UserId);

                // new refresh token
                await userManager.RemoveAuthenticationTokenAsync(user, AuthOptions.ISSUER, "RefreshToken");
                var refresh_jwtToken = await userManager.GenerateUserTokenAsync(user, AuthOptions.ISSUER, "RefreshToken");
                await userManager.SetAuthenticationTokenAsync(user, AuthOptions.ISSUER, "RefreshToken", refresh_jwtToken);

                // new access token
                string access_jwtToken = TokenFactory.GetAccessToken(user);

                return new JsonResult(new { access_jwtToken, refresh_jwtToken });
            }

            return NotFound();
        }

        // POST: api/authentication/reset-password
        [HttpPost("/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            bool succeeded = false;

            if (user != null)
            {
                var resetPassToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, resetPassToken, request.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new { result.Succeeded });
                }

                return StatusCode(500, new { result.Succeeded });
            }

            return NotFound(succeeded);
        }

        // POST: api/authentication/check-code
        [HttpPost("/check-code")]
        public async Task<IActionResult> CheckCode([FromBody] CheckCodeRequest request)
        {
            var code = await codeService.Get(request.Code, request.Email);
            bool succeeded = false;

            if (code != null)
            {
                codeService.RemoveRange(request.Email);

                succeeded = true;
                return Ok(new { succeeded });
            }

            return StatusCode(400, new { succeeded });
        }

        // POST: api/authentication/send-code
        [HttpPost("/send-code")]
        public void SendSecretCodeToUser([FromBody] string email)
        {
            if (codeService.Count(email) < 3)
            {
                var secretString = SecretString.GetSecretString();

                mailService.SendEmail(email, secretString, "Your personal code");

                var code = new SecretCode() { Code = secretString, Email = email };

                codeService.Add(code);
            }
        }
    }
}