using Microsoft.AspNetCore.Mvc;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        IRouteService routeService;

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
            var route = routeService.GetRouteById(id);

            if (route == null)
            {
                return NotFound();
            }

            return new ObjectResult(route);
        }

        [HttpPut("{id}")]
        public IActionResult PutRoute(int id, RouteDto routeDto)
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

        [HttpPost]
        public IActionResult PostRoute(RouteDto routeDto)
        {
            if (routeDto == null)
            {
                return BadRequest();
            }

            routeService.Create(routeDto);

            return Ok(routeDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoute(int id)
        {
            var route = routeService.GetRouteById(id);

            if (route == null)
            {
                return NotFound();
            }

            routeService.Delete(id);

            return Ok(route);
        }
    }
}
