using System;
using Microsoft.AspNetCore.Mvc;
using ETicket.DataAccess.Domain.Entities;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;
using log4net;
using System.Reflection;

namespace ETicket.WebAPI.Controllers
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : BaseAPIController
    {
        IDocumentService documentService;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DocumentsController(IDocumentService iDocumentService)
        {
            documentService = iDocumentService;
        }

        [HttpGet]
        public IActionResult GetDocuments()
        {
            log.Info(nameof(DocumentsController.GetDocuments));

            return Ok(documentService.GetDocuments());
        }
                
        [HttpGet("{id}")]
        public IActionResult GetDocument(Guid id)
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
        public  IActionResult UpdateDocument(Guid id, DocumentDto documentDto)
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
        public IActionResult PostDocument(DocumentDto documentDto)
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
