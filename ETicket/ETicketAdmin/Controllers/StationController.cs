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
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class StationController : Controller
    {
        private readonly IStationService service;

        public StationController(IStationService service)
        {
            this.service = service;
        }

        // GET: Station
        public IActionResult Index()
        {
            try
            {
                return View(service.GetAll());
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Station/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var station = service.Get((Int32)id);

                if (station == null)
                {

                    return NotFound();
                }
                else
                {
                    return View(station);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Station/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Station/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StationDto stationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Create(stationDto);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return View(stationDto);
        }

        // GET: Station/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var station = service.Get((Int32)id);

                if (station == null)
                {

                    return NotFound();
                }
                else
                {

                    return View(station);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Station/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, StationDto stationDto)
        {
            if(id != stationDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.Update(stationDto);
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(stationDto);
        }

        // GET: Station/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var station = service.Get((Int32)id);

                if (station == null)
                {

                    return NotFound();
                }
                else
                {
                    return View(station);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Station/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                service.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
