using System;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using log4net;

namespace ETicket.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser")]
    public class AreasController : Controller
    {
        private readonly IAreaService areaService;
        //private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var areas = areaService.GetAreas();
                
                return View(areas);
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                //logger.Warn(nameof(AreaController.Details) + " id is null");

                return NotFound();
            }

            try
            {
                var area = areaService.GetAreaById(id.Value);

                if (area == null)
                {
                    //logger.Warn(nameof(AreaController.Details) + " area is null");

                    return NotFound();
                }

                return View(area);
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AreaDto areaDto)
        {
            if (!ModelState.IsValid)
            {                
                return View(areaDto);
            }

            try
            {
                areaService.Create(areaDto);                
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                //logger.Warn(nameof(AreaController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var areaDto = areaService.GetAreaDtoById(id.Value);

                if (areaDto == null)
                {
                    //logger.Warn(nameof(AreaController.Edit) + " area is null");

                    return NotFound();
                }

                return View(areaDto);
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AreaDto areaDto)
        {
            if (id != areaDto.Id)
            {
                //logger.Warn(nameof(AreaController.Edit) + " id is not equal to areaDto.Id");

                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    areaService.Update(areaDto);

                    return RedirectToAction(nameof(Index));
                }
                
                return View(areaDto);
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }
        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                //logger.Warn(nameof(AreaController.Edit) + " id is null");

                return NotFound();
            }

            try
            {
                var area = areaService.GetAreaById(id.Value);

                if (area == null)
                {
                   // logger.Warn(nameof(AreaController.Edit) + " area is null");

                    return NotFound();
                }

                return View(area);
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                areaService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                //logger.Error(e);

                return BadRequest();
            }
        }
    }
}