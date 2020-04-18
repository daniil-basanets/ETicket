using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ETicket.DataAccess.Domain.Interfaces;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class PrivilegesController : Controller
    {
        #region

        private readonly IUnitOfWork unitOfWork;

        #endregion

        public PrivilegesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Privileges
        public IActionResult Index()
        {
            return View(unitOfWork.Privileges.GetAll());
        }

        // GET: Privileges/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = unitOfWork.Privileges.Get((int)id);

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
        public IActionResult Create([Bind("Id,Name,Coefficient")] Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Privileges.Create(privilege);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(privilege);
        }

        // GET: Privileges/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = unitOfWork.Privileges.Get((int)id);
            if (privilege == null)
            {
                return NotFound();
            }
            return View(privilege);
        }

        // POST: Privileges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Coefficient")] Privilege privilege)
        {
            if (id != privilege.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Privileges.Update(privilege);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivilegeExists(privilege.Id))
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
            return View(privilege);
        }

        // GET: Privileges/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = unitOfWork.Privileges.Get((int)id);

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
            unitOfWork.Privileges.Delete(id);
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool PrivilegeExists(int id)
        {
            return unitOfWork.Privileges.Get(id) != null;
        }
    }
}
