using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DocumentController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public DocumentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Authorize(Policy = "RequirePilot&FlightAttendantRole")]
        public async Task<ActionResult<PagedList<Document>>> Get([FromQuery] PaginationParams paginationParams)
        {
            var documents = await _uow.DocumentRepository.GetDocumentsAsync(paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(documents.CurrentPage, documents.PageSize, documents.TotalCount, documents.TotalPages));

            return Ok(documents);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequirePilot&FlightAttendantRole")]
        public async Task<ActionResult<Document>> Detail(int id)
        {
            var document = await _uow.DocumentRepository.GetDocumentAsync(id);
            if (document == null) return NotFound();
            return Ok(document);
        }

        [HttpGet("{id}/file")]
        //[Authorize(Policy = "RequirePilot&FlightAttendantRole")]
        public async Task<IActionResult> GetDocumentFile(int id)
        {
            var document = await _uow.DocumentRepository.GetDocumentFileAsync(id);
            if (document == null) return NotFound("Document not found.");

            var filePath = document.PathFile;
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var contentType = "APPLICATION/octet-stream"; // You should set the proper content type according to your file
            var fileName = Path.GetFileName(filePath); // Or use the DocumentName property from your DocumentDto

            return File(memory, contentType, fileName);
        }


        [HttpPost]
        [Authorize(Policy = "RequirePilot&FlightAttendantRole")]
        public async Task<ActionResult> Create([FromForm] DocumentDto document)
        {
            document.UploadedByUserId = User.GetUserId();
            await _uow.DocumentRepository.CreateDocument(document);

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Policy = "RequirePilot&FlightAttendantRole")]
        public async Task<ActionResult> Update([FromForm] DocumentDto document)
        {
            document.UploadedByUserId = User.GetUserId();
            await _uow.DocumentRepository.UpdateDocument(document);

            if (await _uow.Complete()) return Ok("");

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_uow.DocumentRepository.DeleteDocument(id) == false) return BadRequest("This document is being used or Not Found");

            if (await _uow.Complete()) return Ok();

            return BadRequest();
        }
    }
}
