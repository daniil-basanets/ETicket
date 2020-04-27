using ETicket.ApplicationServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Services.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class PrivilegesController : Controller
    {
        #region

        private readonly IPrivilegeService privilegeService;

        #endregion

        public PrivilegesController(IPrivilegeService privilegeService)
        {
            this.privilegeService = privilegeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(privilegeService.GetPrivileges());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.GetPrivilegeById((int)id);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrivilegeDto privilegeDto)
        {
            if (ModelState.IsValid)
            {
                privilegeService.Create(privilegeDto);

                return RedirectToAction(nameof(Index));
            }

            return View(privilegeDto);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.GetPrivilegeById(id.Value);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PrivilegeDto privilegeDto)
        {
            if (id != privilegeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    privilegeService.Update(privilegeDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!privilegeService.Exists(privilegeDto.Id))
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

            return View(privilegeDto);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.GetPrivilegeById(id.Value);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            privilegeService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
