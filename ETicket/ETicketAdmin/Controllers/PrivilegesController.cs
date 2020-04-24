using ETicket.ApplicationServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.ApplicationServices.Services.PrivilegeService;

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

        // GET: Privileges
        public IActionResult Index()
        {
            return View(privilegeService.GetAll());
        }

        // GET: Privileges/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.Get((int)id);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // GET: Privileges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Privileges/Create
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

        // GET: Privileges/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.Get(id.Value);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // POST: Privileges/Edit/5
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

        // GET: Privileges/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = privilegeService.Get(id.Value);

            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // POST: Privileges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            privilegeService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private bool PrivilegeExists(int id)
        {
            return privilegeService.Exists(id);
        }
    }
}
