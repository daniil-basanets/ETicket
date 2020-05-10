using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var carrier = carrierService.GetDto(id);

            if (carrier == null)
            {
                return NotFound();
            }

            return carrier;
        }

        // PUT: api/Carriers/5
        [HttpPut("{id}")]
        public IActionResult UpdateCarrier(int id, CarrierDto carrierDto)
        {
            if (id != carrierDto.Id)
            {
                return BadRequest();
            }

            try
            {
                carrierService.Update(carrierDto);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // POST: api/Carriers
        [HttpPost]
        public ActionResult<Carrier> CreateCarrier(CarrierDto carrierDto)
        {
            carrierService.Create(carrierDto);

            return CreatedAtAction("GetCarrier", carrierDto);

            // return CreatedAtAction("GetCarrier", new { id = carrier.Id }, carrier);
        }

    }
}
