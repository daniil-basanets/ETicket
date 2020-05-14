using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region Private members

        private readonly IUserService userService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public UserController(IUserService UService)
        {
            userService = UService;
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            log.Info(nameof(UserController.GetUser));

            try
            {
                var user = userService.GetUserById(id);

                if (user == null)
                {
                    log.Warn(nameof(UserController.GetUser) + " user is null");

                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, UserDto userDto)
        {
            log.Info(nameof(UserController.UpdateUser));

            try
            {
                if (id != userDto.Id)
                {
                    log.Warn(nameof(UserController.UpdateUser) + " id is not equal to userDto.Id");

                    return BadRequest();
                }

                userService.Update(userDto);

                return NoContent();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: api/user
        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            log.Info(nameof(UserController.CreateUser));

            try
            {
                userService.CreateUser(userDto);

                return Created(nameof(UserController.GetUser), userDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // Get all tickets for user
    }
}