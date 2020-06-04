using ETicket.ApplicationServices.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using System.Reflection;
using System;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class PrivilegesController : Controller
    {
        #region

        private readonly IPrivilegeService privilegeService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public PrivilegesController(IPrivilegeService privilegeService)
        {
            this.privilegeService = privilegeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            log.Info(nameof(PrivilegesController.Index));

            try
            {
                return View(privilegeService.GetPrivileges());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            log.Info(nameof(PrivilegesController.Details));

            if (id == null)
            {
                log.Warn(nameof(PrivilegesController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var privilege = privilegeService.GetPrivilegeById((int)id);

                if (privilege == null)
                {
                    log.Warn(nameof(PrivilegesController.Details) + " privilege is null");

                    return NotFound();
                }

                return View(privilege);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }

        [HttpGet]
        public IActionResult Create()
        {
            log.Info(nameof(PrivilegesController.Create));

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrivilegeDto privilegeDto)
        {
            log.Info(nameof(PrivilegesController.Create));

            try
            {
                if (ModelState.IsValid)
                {
                    privilegeService.Create(privilegeDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(privilegeDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
            
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            log.Info(nameof(PrivilegesController.Edit));

            if (id == null)
            {
                log.Warn(nameof(PrivilegesController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var privilege = privilegeService.GetPrivilegeById(id.Value);

                if (privilege == null)
                {
                    return NotFound();
                }

                return View(privilege);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PrivilegeDto privilegeDto)
        {
            log.Info(nameof(PrivilegesController.Edit));

            if (id != privilegeDto.Id)
            {
                log.Warn(nameof(PrivilegesController.Edit) + " id is not equal to privilegeDto.Id");

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    privilegeService.Update(privilegeDto);
                }
                catch (Exception e)
                {
                    log.Error(e);

                    return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(privilegeDto);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            log.Info(nameof(PrivilegesController.Delete));

            if (id == null)
            {
                log.Warn(nameof(PrivilegesController.Delete) + " id is null");

                return NotFound();
            }

            try
            {
                var privilege = privilegeService.GetPrivilegeById(id.Value);

                if (privilege == null)
                {
                    log.Warn(nameof(PrivilegesController.Delete) + " privilege is null");

                    return NotFound();
                }

                return View(privilege);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            log.Info(nameof(PrivilegesController.DeleteConfirmed));

            try
            {
                privilegeService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
    }
}
