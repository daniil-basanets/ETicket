using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.Admin.Controllers
{
    public class CarriersController : Controller
    {
        #region Private members

        private readonly ICarrierService carrierService;

        #endregion

        public CarriersController(ICarrierService carrierService)
        {
            this.carrierService = carrierService;
        }

        // GET: Carriers
        [HttpGet]
        public IActionResult Index()
        {
            return View(carrierService.GetAll());
        }

        // GET: Carriers/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = carrierService.GetDto((int)id);

            if (carrier == null)
            {
                return NotFound();
            }

            return View(carrier);
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
            if (ModelState.IsValid)
            {
                carrierService.Create(carrierDto);

                return RedirectToAction(nameof(Index));
            }
            return View(carrierDto);
        }

        // GET: Carriers/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = carrierService.GetDto((int)id);

            if (carrier == null)
            {
                return NotFound();
            }

            return View(carrier);
        }

        // POST: Carriers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CarrierDto carrierDto)
        {
            if (id != carrierDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carrierService.Update(carrierDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!carrierService.Exists(carrierDto.Id))
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

            return View(carrierDto);
        }

        // GET: Carriers/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrier = carrierService.GetDto((int)id);

            if (carrier == null)
            {
                return NotFound();
            }

            return View(carrier);
        }

        // POST: Carriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            carrierService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
