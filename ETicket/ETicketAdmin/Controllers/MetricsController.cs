using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Enums;
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
        public IActionResult PassengersByPrivilegesChart()
        {
            log.Info(nameof(MetricsController.PassengersByPrivilegesChart));

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
        public IActionResult PassengersByTimeChart()
        {
            log.Info(nameof(MetricsController.PassengersByTimeChart));

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
        public IActionResult PassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale scale)
        {
            log.Info(nameof(MetricsController.PassengersByTime));

            try
            {
                ChartDto chartDto = metricsService.PassengersByTime(startPeriod, endPeriod, scale);

                return Json(chartDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult MultiRoutesPassengersByTime(DateTime startPeriod, DateTime endPeriod, ChartScale scale, int[] selectedRoutesId)
        {
            log.Info(nameof(MetricsController.PassengersByTime));

            try
            {
                MultiLineChartDto chartDto = metricsService.PassengersByTime(startPeriod, endPeriod, selectedRoutesId, scale);

                return Json(chartDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult TicketsByTicketTypesChart()
        {
            log.Info(nameof(MetricsController.TicketsByTicketTypesChart));

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


        [HttpGet]
        public IActionResult PassengerTrafficByDaysOfWeekChart()
        {
            log.Info(nameof(MetricsController.PassengerTrafficByDaysOfWeekChart));

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
        public IActionResult GetPassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod)
        {
            log.Info(nameof(MetricsController.GetPassengersByDaysOfWeek));

            try
            {
                ChartDto chartDtoTicketsByTicketTypes = metricsService.PassengersByDaysOfWeek(startPeriod, endPeriod);

                return Json(chartDtoTicketsByTicketTypes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetMultiRoutesPassengersByDaysOfWeek(DateTime startPeriod, DateTime endPeriod, int[] selectedRoutesId)
        {
            log.Info(nameof(MetricsController.GetMultiRoutesPassengersByDaysOfWeek));

            try
            {
                MultiLineChartDto chartDto = metricsService.PassengersByDaysOfWeek(startPeriod, endPeriod, selectedRoutesId);

                return Json(chartDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult PassengersByHoursByRoutesChart()
        {
            log.Info(nameof(MetricsController.PassengersByHoursByRoutesChart));

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