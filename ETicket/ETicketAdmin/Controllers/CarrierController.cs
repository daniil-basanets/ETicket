using Microsoft.AspNetCore.Mvc;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class CarrierController : Controller
    {
        #region Private members

        private readonly ICarrierService carrierService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public CarrierController(ICarrierService carrierService)
        {
            this.carrierService = carrierService;
        }

        // GET: Carriers
        [HttpGet]
        public IActionResult Index()
        {
            log.Info(nameof(Index));

            try
            {
                return View(carrierService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Carriers/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            log.Info(nameof(Details));

            if (id == null)
            {
                log.Warn(nameof(Details) + " id is null");

                return NotFound();
            }

            CarrierDto carrierDto;

            try
            {
                carrierDto = carrierService.Get((int)id);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }

            if (carrierDto == null)
            {
                log.Warn(nameof(Details) + " carrierDto is null");

                return NotFound();
            }

            return View(carrierDto);
        }

        // GET: Carriers/Create
        [HttpGet]
        public IActionResult Create()
        {
            log.Info(nameof(Create));

            return View();
        }

        // POST: Carriers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarrierDto carrierDto)
        {
            log.Info(nameof(Create) + ":Post");

            try
            {
                if (ModelState.IsValid)
                {
                    carrierService.Create(carrierDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(carrierDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Carriers/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            log.Info(nameof(Edit));

            return Details(id);
        }

        // POST: Carriers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CarrierDto carrierDto)
        {
            log.Info(nameof(Edit) + ":Post");

            if (id != carrierDto.Id)
            {
                log.Warn(nameof(Edit) + " id is not equal to carrierDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    carrierService.Update(carrierDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(carrierDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: Carriers/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            log.Info(nameof(Delete));

            return Details(id);
        }

        // POST: Carriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            log.Info(nameof(DeleteConfirmed) + ":Post");

            try
            {
                carrierService.Delete(id);
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
