using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/ticket-types")]
    [ApiController]
    [SwaggerTag("Ticket type service")]
    public class TicketTypesController : BaseAPIController
    {
        private readonly ITicketTypeService ticketTypeService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TicketTypesController(ITicketTypeService ticketTypeService)
        {
            this.ticketTypeService = ticketTypeService;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all ticket types", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of ticket types")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        public IActionResult GetTicketTypes()
        {
            logger.Info(nameof(TicketTypesController.GetTicketTypes));
            
            try
            {
                return Ok(ticketTypeService.GetTicketTypes());
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get ticket type by id", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a TicketType object", typeof(TicketTypeDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        public IActionResult GetTicketType([SwaggerParameter("Int", Required = true)] int id)
        {
            logger.Info(nameof(TicketTypesController.GetTicketType));
            
            try
            {
                return Ok(ticketTypeService.GetTicketTypeById(id));
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }
    }
}