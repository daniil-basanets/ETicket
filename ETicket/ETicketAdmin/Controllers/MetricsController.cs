using System;
using System.Reflection;
using ETicket.ApplicationServices.Charts.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult PassengersByPrivileges()
        {
            return Json(metricsService.PassengersByPrivileges(DateTime.Now, DateTime.Now));
        }

        [HttpGet]
        public IActionResult PassengersByTime(DateTime start, DateTime end)
        {
            log.Info(nameof(MetricsController.PassengersByTime));

            try
            {
                ChartDto chartDtoTicketsByTicketTypes = metricsService.PassengersByTime(start, end);

                return Json(chartDtoTicketsByTicketTypes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        public IActionResult GetTicketsByTicketTypes()
        {
            log.Info(nameof(MetricsController.GetTicketsByTicketTypes));

            try
            {
                ChartDto chartDtoTicketsByTicketTypes = metricsService.TicketsByTicketTypes(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);

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