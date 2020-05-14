using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/documenttypes")]
    [ApiController]
    public class DocumentTypesController : ControllerBase
    {
        #region Private members

        private readonly IDocumentTypesService documentTypesService;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public DocumentTypesController(IDocumentTypesService documentTypesService)
        {
            this.documentTypesService = documentTypesService;
        }

        // GET: api/documenttypes
        [HttpGet]
        public IActionResult GetAll()
        {
            log.Info(nameof(GetAll));

            try
            {
                return Ok(documentTypesService.GetDocumentTypes());
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }

        // GET: api/documenttypes/5
        [HttpGet("{id}")]
        public IActionResult GetDocumentTypeById(int id)
        {
            log.Info(nameof(GetDocumentTypeById));

            try
            {
                var documentType = documentTypesService.GetDocumentTypeById(id);

                if (documentType == null)
                {
                    log.Warn(nameof(GetDocumentTypeById) + " document type is null");

                    return NotFound();
                }

                return Ok(documentType);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
    }
}
