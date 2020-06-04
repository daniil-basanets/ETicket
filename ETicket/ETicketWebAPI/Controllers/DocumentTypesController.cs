using ETicket.ApplicationServices.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.WebAPI.Controllers
{
    [Route("api/document-types")]
    [ApiController]
    [SwaggerTag("Document type service")]
    public class DocumentTypesController : BaseAPIController
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
        [SwaggerOperation(Summary = "Get all document types", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of document types")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
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
        [SwaggerOperation(Summary = "Get document type by id", Description = "Allowed: everyone")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a DocumentType object", typeof(DocumentTypeDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(404, "Returns if document type is not found by id")]
        public IActionResult GetDocumentTypeById([SwaggerParameter("Int", Required = true)] int id)
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
