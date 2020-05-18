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

        public IActionResult PassengersByTime()
        {
            ChartDto chartDtoTicketsByTicketTypes = metricsService.PassengersByTime(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, 8);

            return Json(chartDtoTicketsByTicketTypes);
        }

        public IActionResult GetTicketsByTicketTypes()
        {
            ChartDto chartDtoTicketsByTicketTypes = metricsService.TicketsByTicketTypes(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);

            return Json(chartDtoTicketsByTicketTypes);
        }
    }
}