using System;
using System.Reflection;
using ETicket.ApplicationServices.Services.PrivilegeService;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/privileges")]
    [ApiController]
    public class PrivilegesController : ControllerBase
    {
        private readonly IPrivilegeService privilegeService;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PrivilegesController(IPrivilegeService privilegeService)
        {
            this.privilegeService = privilegeService;
        }

        [HttpGet]
        public IActionResult GetPrivileges()
        {
            logger.Info(nameof(PrivilegesController));

            try
            {
                return Ok(privilegeService.GetAll());
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPrivilege(int id)
        {
            logger.Info(nameof(PrivilegesController));

            try
            {
                return Ok(privilegeService.Get(id));
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                return BadRequest();
            }
        }
    }

}