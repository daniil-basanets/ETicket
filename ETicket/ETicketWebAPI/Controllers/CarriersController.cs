using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using ETicket.ApplicationServices.DTOs;
using System;

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
        public ActionResult<IEnumerable<Carrier>> GetAll()
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
        public ActionResult<CarrierDto> GetCarrier(int id)
        {
            try
            {
                var carrier = carrierService.GetDto(id);

                if (carrier == null)
                {
                    log.Warn(nameof(GetCarrier) + " carrier is null");

                    return NotFound();
                }

                return carrier;
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // PUT: api/Carriers/5
        [HttpPut("{id}")]
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
        public ActionResult<Carrier> CreateCarrier(CarrierDto carrierDto)
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
            // return CreatedAtAction("GetCarrier", new { id = carrier.Id }, carrier);
        }

    }
}
