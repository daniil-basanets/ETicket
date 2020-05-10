using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarriersController : ControllerBase
    {
        #region Privat Members

        private readonly ICarrierService carrierService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public CarriersController(ICarrierService carrierService)
        {
            this.carrierService = carrierService;
        }

        // GET: api/Carriers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(carrierService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/Carriers/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCarrier(int id)
        {
            try
            {
                var carrier = carrierService.GetDto(id);

                if (carrier == null)
                {
                    log.Warn(nameof(GetCarrier) + " carrier is null");

                    return NotFound();
                }

                return Ok(carrier);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // PUT: api/Carriers/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateCarrier(int id, CarrierDto carrierDto)
        {
            try
            {
                if (id != carrierDto.Id)
                {
                    log.Warn(nameof(UpdateCarrier) + " id is not equal to carrierDto.Id");

                    return BadRequest();
                }

                carrierService.Update(carrierDto);

                return NoContent();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: api/Carriers
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateCarrier(CarrierDto carrierDto)
        {
            try
            {
                carrierService.Create(carrierDto);

                return Created(nameof(GetCarrier), carrierDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

    }
}
