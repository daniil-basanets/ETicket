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
using ETicket.WebAPI.Services.TicketVerifyService;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketVerificationsController : ControllerBase
    {
        private readonly ITicketVerificationService verificationService;
        private readonly ITicketVerifyService verifyService;

        public TicketVerificationsController(ITicketVerificationService service, ITicketVerifyService verifyService)
        {
            this.verificationService = service;
            this.verifyService = verifyService;
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
        public IActionResult VerifyTicket(VerifyTicketRequest request)
        {
            
            return Ok(verifyService.VerifyTicket(request));
        }
    }
}
