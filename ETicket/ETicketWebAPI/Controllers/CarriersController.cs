using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using log4net;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/carriers")]
    [ApiController]
    [SwaggerTag("Carrier service")]
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
        [SwaggerOperation(Summary = "Get all carriers", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of carriers")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
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
        [SwaggerOperation(Summary = "Get carrier by id", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a Carrier object", typeof(CarrierDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(404, "Returns if carrier is not found by id")]
        public IActionResult GetCarrierById([SwaggerParameter("Int", Required = true)] int id)
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
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update carrier", Description = "Allowed: Admin")]
        [SwaggerResponse(204, "Returns if everything is correct, without content")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult UpdateCarrier([SwaggerParameter("Int", Required = true)] int id, [FromBody, SwaggerRequestBody("Carrier payload", Required = true)] CarrierDto carrierDto)
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
        [Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "Create carrier", Description = "Allowed: Admin")]
        [SwaggerResponse(201, "Returns if carrier is created")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult CreateCarrier([FromBody, SwaggerRequestBody("Carrier payload", Required = true)] CarrierDto carrierDto)
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
