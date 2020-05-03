using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.WebAPI.Models.TicketVerification;
using ETicket.WebAPI.Services.TicketsService;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketVerificationsController : ControllerBase
    {
        private readonly ITicketVerificationService verificationService;
        private readonly ITicketVerifyService ticketService;

        public TicketVerificationsController(ITicketVerificationService service, ITicketVerifyService verifyService)
        {
            this.verificationService = service;
            this.ticketService = verifyService;
        }

        [HttpGet("{ticketid}/verifications/lastitems")]
        public IActionResult GetLastTicketVerification(Guid ticketId, [FromQuery]int count = 1)
        {
            var ticketVerification = verificationService
                    .GetAll()
                    .Where(t => t.TicketId == ticketId)
                    .OrderByDescending(t => t.VerificationUTCDate)
                    .Take(count);

            return Ok(ticketVerification);
        }

        [HttpPost]
        [Route("{ticketid}/verifications")]
        public IActionResult VerifyTicket(Guid ticketId, [FromBody]VerifyTicketInfo request)
        {            
            return Ok(ticketService.VerifyTicket(ticketId, request));
        }
    }
}
