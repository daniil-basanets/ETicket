using System.Collections.Generic;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreaService areaService;

        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }

        // GET: api/Areas
        [HttpGet]
        public IEnumerable<Area> GetAreas()
        {
            return areaService.GetAll();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public ActionResult<Area> GetArea(int id)
        {
            var area = areaService.Get(id);

            if (area == null)
            {
                return NotFound();
            }

            return area;
        }
    }
}