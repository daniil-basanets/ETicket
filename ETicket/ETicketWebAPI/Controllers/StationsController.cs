using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Extensions;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/stations")]
    [ApiController]
    [SwaggerTag("Station service")]
    public class StationsController : BaseAPIController
    {
        #region Private members

        private readonly IStationService stationService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public StationsController(IStationService stationService)
        {
            this.stationService = stationService;
        }

        // GET: api/stations
        [HttpGet]
        [SwaggerOperation(Summary = "Get all stations", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of stations")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        public IActionResult GetStations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            log.Info(nameof(StationsController.GetStations));

            try
            {
                var stationPage = stationService
                        .GetAll()
                        .ToPage(pageNumber, pageSize);

                return Ok(stationPage);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/stations/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get station by id", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a Station object", typeof(StationDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(404, "Returns if station is not found by id")]
        public IActionResult GetStation([SwaggerParameter("Int", Required = true)] int id)
        {
            log.Info(nameof(StationsController.GetStation));

            try
            {
                var station = stationService.Get(id);

                if (station == null)
                {
                    log.Warn(nameof(StationsController.GetStation) + " station is null");

                    return NotFound();
                }

                return Ok(station);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}