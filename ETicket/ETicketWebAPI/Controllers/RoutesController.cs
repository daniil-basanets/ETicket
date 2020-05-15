using Microsoft.AspNetCore.Mvc;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;
using System;

namespace ETicket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/routes")]
    public class RoutesController : ControllerBase
    {
        IRouteService routeService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RoutesController(IRouteService iRouteService)
        {
            routeService = iRouteService;
        }

        [HttpGet]
        public IActionResult GetRoutes()
        {
            return Ok(routeService.GetRoutes());
        }

        [HttpGet("{id}")]
        public IActionResult GetRoute(int id)
        {
            try
            {
                var route = routeService.GetRouteById(id);

                if (route == null)
                {
                    return NotFound();
                }

                return new ObjectResult(route);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }           
        }

        [HttpPut("{id}")]
        public IActionResult PutRoute(int id, RouteDto routeDto)
        {
            try
            {
                if (id != routeDto.Id | routeDto == null)
                {
                    return BadRequest();
                }

                if (!routeService.Exists(id))
                {
                    return NotFound();
                }

                routeService.Update(routeDto);

                return Ok(routeDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }

        [HttpPost]
        public IActionResult PostRoute(RouteDto routeDto)
        {
            try
            {
                if (routeDto == null)
                {
                    return BadRequest();
                }

                routeService.Create(routeDto);

                return Ok(routeDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoute(int id)
        {
            try
            {
                var route = routeService.GetRouteById(id);

                if (route == null)
                {
                    return NotFound();
                }

                routeService.Delete(id);

                return Ok(route);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
    }
}
