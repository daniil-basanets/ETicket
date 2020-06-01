using System;
using System.Reflection;
using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETicket.Admin.Controllers
{
    public class MetricsController : Controller
    {
        #region Private members

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMetricsService metricsService;

        #endregion

        public MetricsController(IMetricsService metricsService)
        {
            this.metricsService = metricsService;
        }

        public IActionResult Index()
        {
            log.Info(nameof(MetricsController.Index));

            try
            {
                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod)
        {
            log.Info(nameof(MetricsController.PassengersByTime));
            
            try
            {
                return Json(metricsService.PassengersByPrivileges(startPeriod, endPeriod));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult PassengersByTime(DateTime startPeriod, DateTime endPeriod)
        {
            log.Info(nameof(MetricsController.PassengersByTime));

            try
            {
                ChartDto chartDtoTicketsByTicketTypes = metricsService.PassengersByTime(startPeriod, endPeriod);

                return Json(chartDtoTicketsByTicketTypes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        public IActionResult GetTicketsByTicketTypes(DateTime startPeriod, DateTime endPeriod)
        {
            log.Info(nameof(MetricsController.GetTicketsByTicketTypes));
        
            try
            {
                ChartDto chartDtoTicketsByTicketTypes = metricsService.TicketsByTicketTypes(startPeriod, endPeriod);

                return Json(chartDtoTicketsByTicketTypes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        public IActionResult GetPassengersByHoursByRoutes(DateTime selectedDate)
        {
            log.Info(nameof(MetricsController.GetTicketsByTicketTypes));

            try
            {
                ChartTableDto chartDtoTicketsByTicketTypes = metricsService.PassengersByHoursByRoutes(selectedDate, null);

                var jsonString = JsonConvert.SerializeObject(chartDtoTicketsByTicketTypes, Formatting.Indented);

                var result = new ObjectResult(jsonString);
                result.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status200OK;

                return result;
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}