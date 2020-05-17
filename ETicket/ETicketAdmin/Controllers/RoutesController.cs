using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ETicket.ApplicationServices.Services;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class RoutesController : Controller
    {
        private readonly IRouteService routeService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RoutesController(IRouteService routeService)
        {
            this.routeService = routeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            log.Info(nameof(RoutesController.Index));

            try
            {
                var routes = routeService.GetRoutes();

                return View(routes);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            log.Info(nameof(RoutesController.Details));

            try
            {
                if (id == null)
                {
                    log.Warn(nameof(RoutesController.Details) + " id is null");

                    return NotFound();
                }

                var route = routeService.GetRouteById(id.Value);

                if (route == null)
                {
                    log.Warn(nameof(RoutesController.Details) + " route is null");

                    return NotFound();
                }

                return View(route);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            log.Info(nameof(RoutesController.Create));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RouteDto routeDto)
        {
            log.Info(nameof(RoutesController.Create));

            try
            {
                if (ModelState.IsValid)
                {
                    routeService.Create(routeDto);

                    return RedirectToAction(nameof(Index));
                }
                return View(routeDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            log.Info(nameof(RoutesController.Edit));

            try
            {
                if (id == null)
                {
                    log.Warn(nameof(RoutesController.Edit) + " id is null");

                    return NotFound();
                }

                var route = routeService.GetRouteById(id.Value);

                if (route == null)
                {
                    log.Warn(nameof(RoutesController.Edit) + " route is null");

                    return NotFound();
                }

                return View(route);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RouteDto routeDto)
        {
            log.Info(nameof(RoutesController.Edit));

            if (id != routeDto.Id)
            {
                log.Warn(nameof(RoutesController.Edit) + " id isn't equal to routeDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    routeService.Update(routeDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(routeDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            log.Info(nameof(RoutesController.Delete));

            if (id == null)
            {
                log.Warn(nameof(RoutesController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var route = routeService.GetRouteById(id.Value);

                if (route == null)
                {
                    return NotFound();
                }

                return View(route);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            log.Info(nameof(RoutesController.DeleteConfirmed));

            try
            {
                routeService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}
