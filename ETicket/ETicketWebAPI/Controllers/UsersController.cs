using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Collections.Generic;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [SwaggerTag("User service")]
    public class UsersController : ControllerBase
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

        // GET: api/users/{id}/tickets
        [HttpGet("{userid}/tickets")]
        [SwaggerOperation(Summary = "Get all tickets for concrete user", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Ok", typeof(IEnumerable<TicketDto>))]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(401, "Unauthorized")]
        public IActionResult GetTicketsByUser(Guid userId)
        {
            log.Info(nameof(GetTicketsByUser));

            try
            {
                var tickets = ticketService.GetTicketsByUserId(userId);

                return Ok(tickets);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by id", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Ok", typeof(UserDto))]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(404, "Not found")]
        [SwaggerResponse(401, "Unauthorized")]
        public IActionResult GetUser(Guid id)
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
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update user", Description = "Allowed: authorized user")]
        [SwaggerResponse(204, "Ok, without content")]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(401, "Unauthorized")]
        public IActionResult UpdateUser(Guid id, [FromBody, SwaggerRequestBody("User payload", Required = true)] UserDto userDto)
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
        [SwaggerResponse(201, "Ok, created")]
        [SwaggerResponse(400, "Bad request")]
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
