using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class StationController : BaseMvcController
    {
        #region Private members

        private readonly IStationService stationService;
        private readonly IAreaService areaService;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public StationController(IStationService stationService, IAreaService areaService)
        {
            this.stationService = stationService;
            this.areaService = areaService;
        }

        // GET: Station
        public IActionResult Index()
        {
            log.Info(nameof(StationController.Index));

            try
            {
                ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name");

                return View(stationService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Station/Details/5
        public IActionResult Details(int? id)
        {
            log.Info(nameof(StationController.Details));

            if (id == null)
            {
                log.Warn(nameof(StationController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var station = stationService.Get(id.Value);

                if (station == null)
                {
                    log.Warn(nameof(StationController.Details) + " station is null");

                    return NotFound();
                }
                else
                {
                    return View(station);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Station/Create
        public IActionResult Create()
        {
            log.Info(nameof(StationController.Create));

            try
            {
                ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Station/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StationDto stationDto)
        {
            log.Info(nameof(StationController.Create) + " POST");

            try
            {
                if (ModelState.IsValid)
                {

                    stationService.Create(stationDto);

                    return RedirectToAction(nameof(Index));
                }

                ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name", stationDto.AreaId);

                return View(stationDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Station/Edit/5
        public IActionResult Edit(int? id)
        {
            log.Info(nameof(StationController.Edit));

            if (id == null)
            {
                log.Warn(nameof(StationController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var station = stationService.Get(id.Value);

                if (station == null)
                {
                    log.Warn(nameof(StationController.Edit) + " station is null");

                    return NotFound();
                }
                else
                {
                    ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name");

                    return View(station);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Station/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, StationDto stationDto)
        {
            log.Info(nameof(StationController.Edit) + " POST");

            if (id != stationDto.Id)
            {
                log.Warn(nameof(StationController.Edit) + " id is not equal to stationDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    stationService.Update(stationDto);

                    return RedirectToAction(nameof(Index));
                }

                ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name", stationDto.AreaId);

                return View(stationDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Station/Delete/5
        public IActionResult Delete(int? id)
        {
            log.Info(nameof(StationController.Delete));

            if (id == null)
            {
                log.Warn(nameof(StationController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var station = stationService.Get(id.Value);

                if (station == null)
                {
                    log.Warn(nameof(StationController.Delete) + " station is null");

                    return NotFound();
                }
                else
                {
                    return View(station);
                }
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: Station/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            log.Info(nameof(StationController.DeleteConfirmed));

            try
            {
                stationService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetStationList()
        {
            log.Info(nameof(StationController.GetStationList));

            try
            {
                var stations = stationService.GetAll();

                return Json(stations);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}