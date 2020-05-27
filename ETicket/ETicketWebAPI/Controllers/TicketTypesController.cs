using System;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/tickettypes")]
    [ApiController]
    public class TicketTypesController : BaseAPIController
    {
        private readonly ITicketTypeService ticketTypeService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TicketTypesController(ITicketTypeService ticketTypeService)
        {
            this.ticketTypeService = ticketTypeService;
        }
        
        [HttpGet]
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
        public IActionResult GetTicketType(int id)
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