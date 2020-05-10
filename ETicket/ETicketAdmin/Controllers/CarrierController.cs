using Microsoft.AspNetCore.Mvc;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class CarriersController : Controller
    {
        #region Private members

        private readonly ICarrierService carrierService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public CarriersController(ICarrierService carrierService)
        {
            this.carrierService = carrierService;
        }

        // GET: Carriers
        [HttpGet]
        public IActionResult Index()
        {
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
            if (id == null)
            {
                log.Warn(nameof(Details) + " id is null"););

                return NotFound();
            }

            CarrierDto carrierDto;

            try
            {
                carrierDto = carrierService.GetDto((int)id);
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
            return View();
        }

        // POST: Carriers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarrierDto carrierDto)
        {
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
            if (id == null)
            {
                log.Warn(nameof(Edit) + " id is null"););

                return NotFound();
            }

            CarrierDto carrierDto;

            try
            {
                carrierDto = carrierService.GetDto((int)id);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }

            if (carrierDto == null)
            {
                log.Warn(nameof(Edit) + " carrierDto is null");

                return NotFound();
            }

            return View(carrierDto);
        }

        // POST: Carriers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CarrierDto carrierDto)
        {
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
            if (id == null)
            {
                log.Warn(nameof(Details) + " id is null"););

                return NotFound();
            }

            CarrierDto carrierDto;

            try
            {
                carrierDto = carrierService.GetDto((int)id);
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

        // POST: Carriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
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
