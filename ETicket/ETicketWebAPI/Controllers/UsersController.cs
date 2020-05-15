using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
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
    }
}
