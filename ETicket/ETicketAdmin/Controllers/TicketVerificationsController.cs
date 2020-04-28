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

namespace ETicket.Admin.Controllers
{
    public class TicketVerificationsController : Controller
    {
        private readonly HttpClient httpClient;

        public TicketVerificationsController()
        {
            httpClient = new HttpClient();
        }

        // GET: TicketVerifications
        public async Task<IActionResult> Index()
        {
            var request = TicketVerificationEndpoints.Get;
            var response = await httpClient.GetAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ticketVerifications = JsonConvert.DeserializeObject<IEnumerable<TicketVerification>>(jsonResponse);

            return View(ticketVerifications);
        }

        // GET: TicketVerifications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = TicketVerificationEndpoints.Get + "/" + id.Value.ToString();
            //var content = new StringContent(id.Value.ToString());
            var response = await httpClient.GetAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ticketVerification = JsonConvert.DeserializeObject<TicketVerification>(jsonResponse);
            if (ticketVerification == null)
            {
                return NotFound();
            }

            return View(ticketVerification);
        }
    }
}
