using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicket.Admin.Controllers
{
    public class TransportsController : Controller
    {
        #region

        private readonly ITransportService transportService;

        private readonly IRouteService routeService;

        private readonly ICarrierService carrierService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public TransportsController(ITransportService transportService, IRouteService routeService, ICarrierService carrierService)
        {
            this.transportService = transportService;
            this.routeService = routeService;
            this.carrierService = carrierService;
        }

        // GET: Transport
        public IActionResult Index()
        {
            log.Info(nameof(Index));

            try
            {
                return View(transportService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }

        }

        // GET: Transport/Details/5
        public IActionResult Details(int? id)
        {
            log.Info(nameof(Details));

            if (id == null)
            {
                log.Warn(nameof(TransportsController.Details) + " id is null");

                return NotFound();
            }
            try
            {
                var transport = transportService.Get((Int32)id);

                if (transport == null)
                {
                    log.Warn(nameof(TransportsController.Details) + " transport is null");

                    return NotFound();
                }
                else
                {
                    return View(transport);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return NotFound();
            }
        }

        // GET: Transport/Create
        public IActionResult Create()
        {
            log.Info(nameof(TransportsController.Create));

            try
            {
                ViewData["RouteId"] = new SelectList(routeService.GetRoutes().Select(a => new { a.Id, a.Number }), "Id", "Number");
                ViewData["CarrierId"] = new SelectList(carrierService.GetAll().Select(a => new { a.Id, a.Name }), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Transport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransportDto transportDto)
        {
            log.Info(nameof(Create) + ": Post");

            if (ModelState.IsValid)
            {
                try
                {
                    transportService.Create(transportDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    log.Error(e);

                    return BadRequest();
                }
            }

            return View(transportDto);
        }

        // GET: Transport/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            log.Info(nameof(TransportsController.Edit));

            if (id == null)
            {
                log.Warn(nameof(TransportsController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var transport = transportService.Get(id.Value);

                if (transport == null)
                {
                    log.Warn(nameof(TransportsController.Edit) + " transport is null");

                    return NotFound();
                }
                else
                {
                    ViewData["RouteId"] = new SelectList(routeService.GetRoutes().Select(a => new { a.Id, a.Number }), "Id", "Number");
                    ViewData["CarrierId"] = new SelectList(carrierService.GetAll().Select(a => new { a.Id, a.Name }), "Id", "Name");

                    return Details(id);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Transport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TransportDto transportDto)
        {
            log.Info(nameof(Edit) + ": Post");

            if (id != transportDto.Id)
            {
                log.Warn(nameof(Edit) + " id is not equal to transportDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                
                    transportService.Update(transportDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(transportDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Transport/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            log.Info(nameof(TransportsController.Delete));

            if (id == null)
            {
                log.Warn(nameof(TransportsController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var transport = transportService.Get(id.Value);

                if (transport == null)
                {
                    log.Warn(nameof(TransportsController.Delete) + " transport is null");

                    return NotFound();
                }
                else
                {
                    return View(transport);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Transport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            log.Info(nameof(DeleteConfirmed) + ": Post");

            try
            {
                transportService.Delete(id);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

