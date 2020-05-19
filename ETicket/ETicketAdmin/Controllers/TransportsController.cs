using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicket.Admin.Controllers
{
    public class TransportsController : Controller
    {
        #region

        private readonly ITransportService transportService;
        private readonly ICarrierService carrierService;
        private readonly IRouteService routeService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public TransportsController(ITransportService transportService, ICarrierService carrierService, IRouteService routeService)
        {
            this.transportService = transportService;
            this.carrierService = carrierService;
            this.routeService = routeService;
        }

        // GET: Transport
        public IActionResult Index()
        {
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
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number");
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name");

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
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number");
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name");

            return View();
        }

        // POST: Transport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransportDto transportDto)
        {
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number", transportDto?.Id);
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name", transportDto?.Id);
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
            log.Info(nameof(Edit));
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number");
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name");


            return Details(id);
        }

        // POST: Transport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TransportDto transportDto)
        {
            log.Info(nameof(Edit) + ":Post");
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number", transportDto?.Id);
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name", transportDto?.Id);

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
            ViewData["RouteId"] = new SelectList(routeService.GetRoutes(), "Id", "Number");
            ViewData["CarrierId"] = new SelectList(carrierService.GetAll(), "Id", "Name");
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
            log.Info(nameof(DeleteConfirmed) + ":Post");

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

