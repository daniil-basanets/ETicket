using System;
using System.Reflection;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class AreasController : Controller
    {
        #region Privite fields

        private readonly IAreaService areaService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            logger.Info(nameof(AreasController.Index));
            
            try
            {
                return View(areaService.GetAreas());
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Details(int? id)
        {
            logger.Info(nameof(AreasController.Index));
            
            if (id == null)
            {
                logger.Warn(nameof(AreasController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var areaDto = areaService.GetAreaById(id.Value);  
                
                if (areaDto == null)
                {
                    logger.Warn(nameof(AreasController.Details) + " area is null");
                
                    return NotFound();
                }
                
                return View(areaDto);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                
                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            logger.Info(nameof(AreasController.Create));
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AreaDto areaDto)
        {
            logger.Info(nameof(AreasController.Create));
            
            if (!ModelState.IsValid)
            {                
                return View(areaDto);
            }

            try
            {
                areaService.Create(areaDto);                
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            logger.Info(nameof(AreasController.Edit));
            
            return Details(id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AreaDto areaDto)
        {
            logger.Info(nameof(AreasController.Edit));
            
            if (id != areaDto.Id)
            {
                logger.Warn(nameof(AreasController.Edit) + " id is not equal to area.Id");

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    areaService.Update(areaDto);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);

                    return BadRequest();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            logger.Info(nameof(AreasController.Delete));
            
            return Details(id);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            logger.Info(nameof(AreasController.DeleteConfirmed));
            
            try
            {
                areaService.Delete(id);
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}