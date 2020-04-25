using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.WebAPI.Models.Identity;
using ETicket.WebAPI.Models.Identity.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    //TODO: Add RefreshToken endpoint
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private IdentityResult identityRez;
        private IdentityUser user;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // Registration users
        [HttpPost("registration")]
        public async Task<IActionResult> PostRegistration([FromBody] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                user = new IdentityUser()
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                identityRez = await userManager.CreateAsync(user, request.Password);

                if (identityRez.Succeeded)
                {
                    return Ok(identityRez);
                }
                else
                {
                    return StatusCode(500, "Internal server error.");
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
                var signInRez = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

                if (signInRez.Succeeded)
                {
                    user = await userManager.FindByNameAsync(request.UserName);

                    string access_jwtToken = TokenFactory.GetAccessToken(user);// TODO: Add refresh token
                    return new JsonResult(new { access_jwtToken });
                }
                else
                {
                    return StatusCode(500, "Internal server error.");
                }
            }

            return StatusCode(400, "Bad request.");
        }
    }
}