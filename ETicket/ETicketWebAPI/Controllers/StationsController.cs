using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.ApplicationServices.DTOs;
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
    public class StationsController : ControllerBase
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
        [SwaggerResponse(200, "Ok", typeof(IEnumerable<Station>))]
        [SwaggerResponse(400, "Bad request")]
        public IActionResult GetStations()
        {
            log.Info(nameof(StationsController.GetStations));

            try
            {
                return Ok(stationService.GetAll());
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
        [SwaggerResponse(200, "Ok", typeof(StationDto))]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(404, "Not found")]
        public IActionResult GetStation(int id)
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