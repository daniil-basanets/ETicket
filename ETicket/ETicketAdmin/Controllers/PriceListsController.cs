using System;
using Microsoft.AspNetCore.Mvc;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using ETicket.DataAccess.Domain.Entities;
using System.Linq;

namespace ETicket.Admin.Controllers
{
    public class PriceListsController : Controller
    {
        #region

        private readonly IPriceListService priceListService;

        private readonly IAreaService areaService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public PriceListsController(IPriceListService priceListService, IAreaService areaService)
        {
            this.priceListService = priceListService;
            this.areaService = areaService;
        }

        // GET: PriceList
        public IActionResult Index()
        {
            try
            {
               ViewData["AreaId"] = new SelectList(areaService.GetAreas(), "Id", "Name");

                return View(priceListService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: PriceList/Create
        public IActionResult Create()
        {
            log.Info(nameof(PriceListsController.Create));

            try
            {
                ViewData["AreaId"] = new SelectList(areaService.GetAreas().Select(a => new { a.Id, a.Name }), "Id", "Name");

                return View();
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // POST: PriceList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PriceListDto priceListDto)
        {
            log.Info(nameof(PriceListsController.Create) + ":Post");

            try
            {
                ViewData["AreaId"] = new SelectList(areaService.GetAreas().Select(a => new { a.Id, a.Name }), "Id", "Name");

                if (ModelState.IsValid)
                {
                    priceListService.Create(priceListDto);

                    return RedirectToAction(nameof(Index));
                }

                return View(priceListDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}