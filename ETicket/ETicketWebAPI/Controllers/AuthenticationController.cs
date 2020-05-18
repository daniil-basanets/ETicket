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
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            log.Info(nameof(CheckEmail));

            try
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

                log.Warn(nameof(AuthenticationController.CheckEmail) + " Bad request");
                return StatusCode(400, new { ModelState.IsValid });
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // Registration user
        // POST: api/authentication/registration
        [HttpPost("/registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
        {
            log.Info(nameof(Registration));

            try
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
                        log.Warn(nameof(AuthenticationController.Registration) + $" Identity result is {identityResult.Succeeded}");

                        return StatusCode(500, new { identityResult.Succeeded });
                    }
                }

                log.Warn(nameof(AuthenticationController.Registration) + $" Bad request");
                return StatusCode(400, new { ModelState.IsValid });
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // Login user
        // POST: api/authentication/token
        [HttpPost("/token")]
        public async Task<IActionResult> GetToken([FromBody] AuthenticationRequest request)
        {
            log.Info(nameof(GetToken));

            try
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

                    log.Warn(nameof(AuthenticationController.GetToken) + $" SignIn result is {signInResult.Succeeded}");
                    return StatusCode(500, new { signInResult.Succeeded });
                }

                log.Warn(nameof(AuthenticationController.GetToken) + $" Bad request");
                return StatusCode(400, new { ModelState.IsValid });
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // Refresh access_jwtToken
        // POST: api/authentication/refresh-token
        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshUserToken([FromBody] string RefreshToken)
        {
            log.Info(nameof(RefreshUserToken));

            try
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

                log.Warn(nameof(AuthenticationController.RefreshUserToken) + $" Token is null");
                return NotFound();
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // POST: api/authentication/reset-password
        [HttpPost("/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            log.Info(nameof(ResetPassword));

            try
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

                    log.Warn(nameof(AuthenticationController.ResetPassword) + $" Server error");
                    return StatusCode(500, new { result.Succeeded });
                }

                log.Warn(nameof(AuthenticationController.ResetPassword) + $" user is null");
                return NotFound(succeeded);
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // POST: api/authentication/check-code
        [HttpPost("/check-code")]
        public async Task<IActionResult> CheckCode([FromBody] CheckCodeRequest request)
        {
            log.Info(nameof(CheckCode));

            try
            {
                var code = await codeService.Get(request.Code, request.Email);
                bool succeeded = false;

                if (code != null)
                {
                    codeService.RemoveRange(request.Email);

                    succeeded = true;
                    return Ok(new { succeeded });
                }

                log.Warn(nameof(AuthenticationController.CheckCode) + $" code is null");
                return StatusCode(400, new { succeeded });
            }
            catch (Exception e)
            {
                log.Error(e);

                return StatusCode(500, new { e.Message });
            }
        }

        // POST: api/authentication/send-code
        [HttpPost("/send-code")]
        public void SendSecretCodeToUser([FromBody] string email)
        {
            log.Info(nameof(SendSecretCodeToUser));

            try
            {
                if (codeService.Count(email) < 3)
                {
                    var secretString = SecretString.GetSecretString();

                    mailService.SendEmail(email, secretString, "Your personal code");

                    var code = new SecretCode() { Code = secretString, Email = email };

                    codeService.Add(code);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}