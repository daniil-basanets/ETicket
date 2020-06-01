using System;
using System.Reflection;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.DataAccess.Domain.Entities;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/privileges")]
    [ApiController]
    [SwaggerTag("Privilege service")]
    public class PrivilegesController : BaseAPIController
    {
        private readonly IPrivilegeService privilegeService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PrivilegesController(IPrivilegeService privilegeService)
        {
            this.privilegeService = privilegeService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all privileges", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything was correct. Contains a list of privileges")]
        [SwaggerResponse(400, "Returns if exception occurred")]
        public IActionResult GetPrivileges()
        {
            logger.Info(nameof(PrivilegesController));

            try
            {
                return Ok(privilegeService.GetPrivileges());
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get privilege by id", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything was correct. Contains a Privilege object", typeof(Privilege))]
        [SwaggerResponse(400, "Returns if exception occurred")]
        [SwaggerResponse(404, "Returns if privilege was not found by id")]
        public IActionResult GetPrivilege([SwaggerParameter("Int", Required = true)] int id)
        {
            logger.Info(nameof(PrivilegesController));

            try
            {
                return Ok(privilegeService.GetPrivilegeById(id));
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }
    }
}