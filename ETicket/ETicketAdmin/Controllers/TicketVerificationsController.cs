using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using System.Net.Http;
using System.Text.Json;
using ETicket.ApplicationServices.DTOs;
using Newtonsoft.Json;
using ETicket.Admin.Models;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using System.Reflection;

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
            try
            {
                return View(ticketVerificationService.GetTicketVerifications());
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
