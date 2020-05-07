using System;
using System.Collections.Generic;
using System.Linq;
using ETicket.ApplicationServices.DTOs;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreasController : ControllerBase
    {
        private readonly IAreaService areaService;
        
        public AreasController(IAreaService areaService)
        {
            this.areaService = areaService;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Area>> GetAreas()
        {
            try
            {
                return areaService.GetAreas().ToList();// спросить про toList
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AreaDto> GetArea(int id)
        {
            var area = areaService.GetAreaDtoById(id);
            
            if (area == null)
            {
                return NotFound();
            }

            return area;
        }

        [HttpDelete("{id}")]
        public ActionResult<Area> DeleteArea(int id)
        {
            var area = areaService.GetAreaById(id);
            
            if (area == null)
            {
                return NotFound();
            }
            
            areaService.Delete(id);
            
            return NoContent(); // response code 204
        }

        [Route("createArea")]
        [HttpPost]
        public ActionResult<Area> CreateArea(AreaDto areaDto)
        {
            if (areaDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            areaService.Create(areaDto);

            return Created(nameof(GetArea),areaDto);
        }

        [Route("updateArea/{id}")]
        [HttpPut]
        public ActionResult UpdateArea(int id, AreaDto areaDto)
        {
            if (id != areaDto.Id)
            {
                return BadRequest();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var area = areaService.GetAreaDtoById(id);

                    if (area == null)
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
            return NoContent();
        }
    }
}