using AutoMapper;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicketAdmin.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class PrivilegesController : Controller
    {
        #region

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        #endregion

        public PrivilegesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
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
        public IActionResult Create(PrivilegeDto privilegeDto)
        {
            if (ModelState.IsValid)
            {
                var privilege = mapper.Map<Privilege>(privilegeDto);

                unitOfWork.Privileges.Create(privilege);
                unitOfWork.Save();
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
                    var privilege = mapper.Map<Privilege>(privilegeDto);

                    unitOfWork.Privileges.Update(privilege);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivilegeExists(privilegeDto.Id))
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
