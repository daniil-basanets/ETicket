using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [SwaggerTag("User service")]
    public class UsersController : BaseAPIController
    {
        #region Private members

        private readonly IUserService userService;
        private readonly ITicketService ticketService;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public UsersController(IUserService userService, ITicketService ticketService)
        {
            this.userService = userService;
            this.ticketService = ticketService;
        }

        // GET: api/users/{email}/tickets
        [Authorize]
        [HttpGet("{email}/tickets")]
        [SwaggerOperation(Summary = "Get all tickets for concrete user", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of user's tickets")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult GetTicketsByUser(string email, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            log.Info(nameof(GetTicketsByUser));

            try
            {
                var ticketPage = ticketService.GetTicketsByUserEmail(email);

                return Json(ticketPage);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/users/5
        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by id", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a User object", typeof(UserDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(404, "Returns if user is not found by id")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult GetUser([SwaggerParameter("Guid", Required = true)] Guid id)
        {
            log.Info(nameof(UsersController.GetUser));

            try
            {
                var user = userService.GetUserById(id);

                if (user == null)
                {
                    log.Warn(nameof(UsersController.GetUser) + " user is null");

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

        // PUT: api/users/5
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update user", Description = "Allowed: authorized user")]
        [SwaggerResponse(204, "Returns if everything is correct, without content")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult UpdateUser([SwaggerParameter("Guid", Required = true)] Guid id, [FromBody, SwaggerRequestBody("User payload", Required = true)] UserDto userDto)
        {
            log.Info(nameof(UsersController.UpdateUser));

            try
            {
                if (id != userDto.Id)
                {
                    log.Warn(nameof(UsersController.UpdateUser) + " id is not equal to userDto.Id");

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

        // POST: api/users
        [HttpPost]
        [SwaggerOperation(Summary = "Create user", Description = "Allowed: everyone")]
        [SwaggerResponse(201, "Returns if user is created")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        public IActionResult CreateUser([FromBody, SwaggerRequestBody("User payload", Required = true)] UserDto userDto)
        {
            log.Info(nameof(UsersController.CreateUser));

            try
            {
                userService.CreateUser(userDto);

                return Created(nameof(UsersController.GetUser), userDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}
