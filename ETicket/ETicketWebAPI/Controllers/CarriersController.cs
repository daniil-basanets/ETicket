using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/carriers")]
    [ApiController]
    public class CarriersController : BaseAPIController
    {
        #region Private members

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
            log.Info(nameof(GetAll));

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
        public IActionResult GetCarrierById(int id)
        {
            log.Info(nameof(GetCarrierById));

            try
            {
                var carrier = carrierService.Get(id);

                if (carrier == null)
                {
                    log.Warn(nameof(GetCarrierById) + " carrier is null");

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
            log.Info(nameof(UpdateCarrier));

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
            log.Info(nameof(CreateCarrier));

            try
            {
                carrierService.Create(carrierDto);

                return Created(nameof(GetCarrierById), carrierDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

    }
}
