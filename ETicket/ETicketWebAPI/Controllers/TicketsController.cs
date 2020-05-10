using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using System.Linq;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        #region Private members

        private readonly ITicketService ticketService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
        public TicketsController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        // GET: api/Tickets?userId=Value
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUserTickets(UserIdDto userIdDto)
        {
            try
            {
                var tickets = ticketService.Get().Where(t => t.UserId == userIdDto.Id);
                return Ok(ticketService.Get());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTicket(Guid id)
        {
            try
            {
                var ticketDto = ticketService.GetDto(id);

                if (ticketDto == null)
                {
                    log.Warn(nameof(GetTicket) + " ticketDto is null");

                    return NotFound();
                }

                return Ok(ticketDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // PUT: api/Tickets/Activate/5
        [HttpPut("/[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Activate(Guid ticketId, Guid userId)
        {
            try
            {
                ticketService.Activate(ticketId, userId);

                return NoContent();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}

