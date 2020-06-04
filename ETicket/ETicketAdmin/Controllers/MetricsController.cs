using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class MetricsController : BaseMvcController
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
        public IActionResult PassengersByPrivileges(DateTime startPeriod, DateTime endPeriod, [FromQuery] int[] selectedRoutesId)
        {
            log.Info(nameof(MetricsController.PassengersByPrivileges));
            
            try
            {
                var chartDtoPassengersByPrivileges = metricsService.PassengersByPrivileges(startPeriod, endPeriod, selectedRoutesId);
                
                return Json(chartDtoPassengersByPrivileges);
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

        public IActionResult GetPassengersByHoursByRoutes(DateTime selectedDate, [FromQuery] int[] selectedRoutesId)
        {
            log.Info(nameof(MetricsController.GetPassengersByHoursByRoutes));

            try
            {
                ChartTableDto chartDtoTicketsByTicketTypes = metricsService.PassengersByHoursByRoutes(selectedDate, selectedRoutesId);

                return Json(chartDtoTicketsByTicketTypes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}