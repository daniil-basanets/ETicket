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
    public class DocumentsController : ControllerBase
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
            return Ok(documentService.GetDocuments());
        }
                
        [HttpGet("{id}")]
        public IActionResult GetDocument(Guid id)
        {
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
        public  IActionResult PutDocument(Guid id, DocumentDto documentDto)
        {
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
