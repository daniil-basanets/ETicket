using System;
using Microsoft.AspNetCore.Mvc;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;

namespace ETicket.Admin.Controllers
{
    public class PriceListsController : Controller
    {
        #region

        private readonly IPriceListService priceListService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public PriceListsController(IPriceListService priceListService)
        {
            this.priceListService = priceListService;
        }

        // GET: PriceList
        public IActionResult Index()
        {
            try
            {
                return View(priceListService.GetAll());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: PriceList/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                log.Warn(nameof(PriceListsController.Details) + " id is null");

                return NotFound();
            }
            try
            {
                var priceList = priceListService.Get((Int32)id);

                if (priceList == null)
                {
                    log.Warn(nameof(PriceListsController.Details) + " priceList is null");

                    return NotFound();
                }
                else
                {
                    return View(priceList);
                }
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
            return View();
        }

        // POST: PriceList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PriceListDto priceListDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    priceListService.Create(priceListDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {

                    log.Error(e);

                    return BadRequest();
                }
            }
            return View(priceListDto);
        }
    }
}
