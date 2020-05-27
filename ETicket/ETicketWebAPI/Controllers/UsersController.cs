using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
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

        // GET: api/users/{id}/tickets
        [HttpGet("{userid}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateUser(Guid id, UserDto userDto)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUser(UserDto userDto)
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
