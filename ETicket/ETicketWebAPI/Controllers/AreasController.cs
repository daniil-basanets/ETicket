using System;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreaService areaService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }
        
        [HttpGet]
        public IActionResult GetAreas()
        {
            logger.Info(nameof(AreasController.GetAreas));
            
            try
            {
                return Ok(areaService.GetAreas());
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetArea(int id)
        {
            logger.Info(nameof(AreasController.GetArea));
            
            try
            {
                return Ok(areaService.GetAreaById(id));
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }
    }
}