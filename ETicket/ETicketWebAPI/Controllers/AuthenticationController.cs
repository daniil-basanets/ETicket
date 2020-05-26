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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [SwaggerTag("Authentication service")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ETicketDataContext context;
        private IdentityResult identityResult;
        private IdentityUser user;
        private readonly IMailService mailService;
        private readonly IUserService ETUserService;
        private readonly ISecretCodeService codeService;

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
        [HttpPost("checkEmail")]
        [SwaggerOperation(Summary = "Check if email exists in data base", Description = "It is used for extra validation on client")]
        [SwaggerResponse(200, "Returns if model was valid. Contains an object with bool variable(exists or not)")]
        [SwaggerResponse(400, "Returns if model was not valid. Contains an object with bool variable(valid or not)")]
        public IActionResult CheckEmail([FromBody, SwaggerRequestBody("Check email payload", Required = true)] RegistrationRequest request)
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
        [HttpPost("registration")]
        [SwaggerOperation(Summary = "Registration")]
        [SwaggerResponse(200, "Returns if model was valid. Contains an object with bool variable(succeeded or not)")]
        [SwaggerResponse(400, "Returns if model was not valid. Contains an object with bool variable(valid or not)")]
        [SwaggerResponse(500, "Server error. Registration failed. Contains an object with bool variable(succeeded or not)")]
        public async Task<IActionResult> Registration([FromBody, SwaggerRequestBody("Registration payload", Required = true)] RegistrationRequest request)
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
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Log in endpoint", Description = "Returns a pair of tokens: access token; refresh token.")]
        [SwaggerResponse(400, "Returns if model was not valid. Contains an object with bool variable(valid or not)")]
        [SwaggerResponse(500, "Server error. Log in failed. Contains an object with bool variable(succeeded or not)")]
        public async Task<IActionResult> GetToken([FromBody, SwaggerRequestBody("Authentication payload", Required = true)] AuthenticationRequest request)
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
        [HttpPost("refreshUserToken")]
        [SwaggerOperation(Summary = "Refresh access token endpoint", Description = "Returns a pair of tokens: access token; refresh token.")]
        [SwaggerResponse(404, "Returns if refresh token was not found")]
        public async Task<IActionResult> RefreshUserToken([FromBody, SwaggerRequestBody("Refresh token", Required = true)] string RefreshToken)
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

        [HttpPost("resetPassword")]
        [SwaggerOperation(Summary = "Reset password", Description = "Calls after verifying special secret code")]
        [SwaggerResponse(200, "Returns if password was reseted. Contains an object with bool variable(succeeded or not)")]
        [SwaggerResponse(404, "Returns if user was not found by email")]
        [SwaggerResponse(500, "Server error. Reset password operation failed. Contains an object with bool variable(succeeded or not)")]
        public async Task<IActionResult> ResetPassword([FromBody, SwaggerRequestBody("Reset password payload", Required = true)] ResetPasswordRequest request)
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

        [HttpPost("checkCode")]
        [SwaggerOperation(Summary = "Check special secret code", Description = "Calls to confirm email and verify code before resetting password")]
        [SwaggerResponse(200, "Returns if code was verified. Contains an object with bool variable(succeeded or not)")]
        [SwaggerResponse(404, "Returns if codel was not found. Contains an object with bool variable(succeeded or not)")]
        public async Task<IActionResult> CheckCode([FromBody, SwaggerRequestBody("Check code payload", Required = true)] CheckCodeRequest request)
        {
            var code = await codeService.Get(request.Code, request.Email);
            bool succeeded = false;

            if (code != null)
            {
                codeService.RemoveRange(request.Email);

                succeeded = true;
                return Ok(new { succeeded });
            }

            return StatusCode(404, new { succeeded });
        }

        [HttpPost("sendCode")]
        [SwaggerOperation(Summary = "Send special secret code", Description = "Calls to send special code to user's email. No more than 3 mails for 1 email.")]
        public void SendSecretCodeToUser([FromBody, SwaggerRequestBody("Email to send code", Required = true)] string email)
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