using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicket.DataAccess.Domain;
using ETicket.DataAccess.Domain.Entities;
using ETicket.DataAccess.Domain.Interfaces;
using ETicket.ApplicationServices.Services;
using ETicket.ApplicationServices.Services.Interfaces;
using ETicket.ApplicationServices.DTOs;

namespace ETicket.WebAPI.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        IDocumentService documentService;

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
            var document = documentService.GetDocumentById(id);

            if (document == null)
            {
                return NotFound();
            }

            return new ObjectResult(document);
        }
               
        [HttpPut("{id}")]
        public  IActionResult PutDocument(Guid id, DocumentDto documentDto)
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
                
        [HttpPost]
        public IActionResult PostDocument(DocumentDto documentDto)
        {
            if (documentDto == null)
            {
                return BadRequest();
            }

            documentService.Create(documentDto);

            return Ok(documentDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(Guid id)
        {
            var document = documentService.GetDocumentById(id);

            if (document == null)
            {
                return NotFound();
            }

            documentService.Delete(id);
            
            return Ok(document);
        }
    }
}
