using System.Collections.Generic;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketTypesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketTypesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/TicketTypes
        [HttpGet]
        public IEnumerable<TicketType> GetTicketTypes()
        {
            var ticketTypes = unitOfWork.TicketTypes.GetAll();

            return ticketTypes;
        }
    }
}