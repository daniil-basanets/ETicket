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

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketVerificationsController : ControllerBase
    {
        private readonly ITicketVerificationService verificationService;

        public TicketVerificationsController(ITicketVerificationService service)
        {
            this.verificationService = service;
        }

        // GET: api/TicketVerifications
        [HttpGet]
        public IActionResult GetTicketVerifications()
        {
            return Ok(verificationService.GetAll());
        }

        // GET: api/TicketVerifications/5
        [HttpGet("{id}")]
        public IActionResult GetTicketVerification(Guid id)
        {
            var ticketVerification = verificationService.Get(id);

            if (ticketVerification == null)
            {
                return NotFound();
            }

            return Ok(ticketVerification);
        }



        [HttpPost]
        [Route("[action]")]
        public IActionResult VerifyTicket([FromBody] VerifyTicketRequest request)
        {

            return Ok(request);
        }
    }
}
