using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.DocumentTypes;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class RoutesController : Controller
    { 
        private readonly RouteService routeService;
        public RoutesController(IUnitOfWork unitOfWork)
        {
            routeService = new RouteService(unitOfWork);
        }

        public IActionResult Index()
        {
            var routes = routeService.Read();

            return View(routes);
        }

        // GET: Routes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = routeService.Read(id.Value);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RouteDto routeDto)
        {
            if (ModelState.IsValid)
            {
                routeService.Create(routeDto);
                routeService.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(routeDto);
        }

        // GET: Routes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = routeService.Read(id.Value);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RouteDto routeDto)
        {
            if (id != routeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    routeService.Update(routeDto);
                    routeService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(routeDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(routeDto);
        }

        // GET: Routes/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = routeService.Read(id.Value);
            
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            routeService.Delete(id);
            routeService.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return routeService.Read(id) != null;
        }
    }
}
