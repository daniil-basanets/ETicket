using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.Admin.Models.DataTables;

namespace ETicket.Admin.Controllers
{
    public class TicketVerificationsController : Controller
    {
        #region Private members

        private readonly ITicketVerificationService ticketVerificationService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public TicketVerificationsController(ITicketVerificationService service)
        {
            this.ticketVerificationService = service;
        }

        // GET: TicketVerifications
        public IActionResult Index()
        {
            log.Info(nameof(TicketVerificationsController.Index));

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
        public IActionResult GetPage([FromQuery]DataTablePagingInfo pagingInfo)
        {
            log.Info(nameof(TicketVerificationsController.GetPage));

            try
            {
                return Json(ticketVerificationService.GetVerificationsPage(pagingInfo));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: TicketVerifications/Details/5
        public IActionResult Details(Guid? id)
        {
            log.Info(nameof(TicketVerificationsController.Details));
            
            if (id == null)
            {
                log.Warn(nameof(TicketVerificationsController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var ticketVerification = ticketVerificationService.GetTicketVerificationById(id.Value);

                if (ticketVerification == null)
                {
                    log.Warn(nameof(TicketVerificationsController.Details) + " user is null");

                    return NotFound();
                }

                return View(ticketVerification);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}
