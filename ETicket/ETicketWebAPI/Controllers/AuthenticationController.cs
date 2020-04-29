using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.DataAccess.Domain;
using ETicket.WebAPI.Models.Identity;
using ETicket.WebAPI.Models.Identity.Requests;
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
                return StatusCode(200, userManager.FindByEmailAsync(request.Email).Result != null);
            }

            return StatusCode(400, "Bad request.");
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
                    return Ok(identityResult);
                }
                else
                {
                    return StatusCode(500, identityResult);
                }
            }

            return StatusCode(400, "Bad request.");
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
                    return StatusCode(500, signInResult);
                }
            }

            return StatusCode(400, "Bad request.");
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
    }
}