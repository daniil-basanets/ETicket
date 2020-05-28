using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/stations")]
    [ApiController]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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