using System;
using Microsoft.AspNetCore.Mvc;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;

namespace ETicket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/documents")]
    [SwaggerTag("Document service")]
    public class DocumentsController : BaseAPIController
    {
        IDocumentService documentService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentsController(IDocumentService iDocumentService)
        {
            documentService = iDocumentService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all documents", Description = "Allowed: Admin")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a list of documents")]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult GetDocuments()
        {
            log.Info(nameof(DocumentsController.GetDocuments));

            return Ok(documentService.GetDocuments());
        }
                
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get document by id", Description = "Allowed: Admin")]
        [SwaggerResponse(200, "Returns if everything is correct. Contains a Document object", typeof(Document))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(404, "Returns if document is not found by id")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult GetDocument([SwaggerParameter("Guid", Required = true)] Guid id)
        {
            log.Info(nameof(DocumentsController.GetDocument));

            try
            {
                var document = documentService.GetDocumentById(id);

                if (document == null)
                {
                    return NotFound();
                }

                return new ObjectResult(document);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }
        }
               
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update document", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if everything is correct", typeof(DocumentDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public  IActionResult UpdateDocument([SwaggerParameter("Guid", Required = true)] Guid id, [FromBody, SwaggerRequestBody("Document payload", Required = true)] DocumentDto documentDto)
        {
            log.Info(nameof(DocumentsController.UpdateDocument));

            try
            {
                if (id != documentDto.Id | documentDto == null)
                {
                    return BadRequest();
                }

                if (!documentService.Exists(id))
                {
                    return NotFound();
                }

                documentService.Update(documentDto);

                return Ok(documentDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
                
        [HttpPost]
        [SwaggerOperation(Summary = "Create document", Description = "Allowed: authorized user")]
        [SwaggerResponse(200, "Returns if document is created", typeof(DocumentDto))]
        [SwaggerResponse(400, "Returns if an exception occurred")]
        [SwaggerResponse(401, "Returns if user is unauthorized")]
        public IActionResult PostDocument([FromBody, SwaggerRequestBody("Document payload", Required = true)] DocumentDto documentDto)
        {
            log.Info(nameof(DocumentsController.PostDocument));

            try
            {
                if (documentDto == null)
                {
                    return BadRequest();
                }

                documentService.Create(documentDto);

                return Ok(documentDto);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(Guid id)
        {
            log.Info(nameof(DocumentsController.DeleteDocument));

            try
            {
                var document = documentService.GetDocumentById(id);

                if (document == null)
                {
                    return NotFound();
                }

                documentService.Delete(id);

                return Ok(document);
            }
            catch (Exception e)
            {
                log.Error(e);

                return BadRequest();
            }            
        }
    }
}
