using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs.Charts;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicket.Admin.Controllers
{
    public class MetricsController : Controller
    {
        #region Private members

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMetricsService metricsService;
        private readonly IRouteService routeService;

        #endregion

        public MetricsController(IMetricsService metricsService, IRouteService routeService)
        {
            this.metricsService = metricsService;
            this.routeService = routeService;
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
            log.Info(nameof(MetricsController.PassengersByPrivileges));
            
            var temp = new[] {2};
            
            var data = metricsService.PassengersByPrivileges(startPeriod, endPeriod, null);

            try
            {
                return Json(data);
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
    }
}