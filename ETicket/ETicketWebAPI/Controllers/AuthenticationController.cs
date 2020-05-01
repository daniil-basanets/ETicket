using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.DataAccess.Domain;
using ETicket.WebAPI.Models.Identity;
using ETicket.WebAPI.Models.Identity.Requests;
using ETicket.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ETicketDataContext context;
        private IdentityResult identityResult;
        private IdentityUser user;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ETicketDataContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        // Check if email exists
        [HttpPost("checkEmail")]
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
        [HttpPost("registration")]
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
                else
                {
                    return StatusCode(500, new { signInResult.Succeeded });
                }
            }

            return StatusCode(400, new { ModelState.IsValid });
        }

        // Refresh access_jwtToken
        [HttpPost("refreshUserToken")]
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
            else
            {
                return NotFound();
            }
        }

        [HttpPost("resetPassword")]
        public IActionResult ResetPassword([FromBody] string email)
        {
            if (userManager.FindByEmailAsync(email).Result != null)
            {
                Random rand = new Random();
                var secretNumber = rand.Next(1000, 9999);
                string secretString = $"{secretNumber}{GetLetter()}{GetLetter()}";

                MailService emailService = new MailService();
                emailService.SendEmail(email, secretString, "Reset password");

                return new JsonResult(new { secretString });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("confirmEmail")]
        public IActionResult ConfirmEmail([FromBody] string email)
        {

            Random rand = new Random();
            var secretNumber = rand.Next(1000, 9999);
            string secretString = $"{secretNumber}{GetLetter()}{GetLetter()}";

            MailService emailService = new MailService();
            emailService.SendEmail(email, secretString, "Confirm email");

            return new JsonResult(new { secretString });
        }

        private char GetLetter()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }
    }
}