using Microsoft.AspNetCore.Mvc;
using RoboticsContainer.Application.DTOs;
using RoboticsContainer.Application.Interfaces;

namespace RoboticsContainer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ContainerEntriesController : ControllerBase
    {
        private readonly IContainerEntryService _service;
        //might have to refactor when the controller starts to get a bit messy
        public ContainerEntriesController(IContainerEntryService service)
        {
            _service = service;
        }

        // POST: api/ContainerEntries
        [HttpPost]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> CreateEntry([FromBody] ContainerEntryRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var entry = await _service.CreateEntryAsync(request);
            return CreatedAtAction(nameof(GetEntryById), new { id = entry.ID }, entry);
        }

        // GET: api/ContainerEntries/{id}
        [HttpGet("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetEntryById(int id)
        {
            var entry = await _service.GetEntryByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        // GET: api/ContainerEntries
        [HttpGet]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetAllEntries()
        {
            var entries = await _service.GetAllEntriesAsync();
            return Ok(entries);
        }

        // PUT: api/ContainerEntries/{id}
        [HttpPut("{id}")]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> UpdateEntry(int id, [FromBody] ContainerEntryRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            await _service.UpdateEntryAsync(id, request);
            return NoContent();
        }

        // DELETE: api/ContainerEntries/{id}
        [HttpDelete("{id}")]
        [Consumes("application/json", "application/xml")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            await _service.DeleteEntryAsync(id);
            return NoContent();
        }

        // GET: api/ContainerEntries/export
        [HttpGet("export")]
        public async Task<IActionResult> ExportEntriesToCsv()
        {
            // Call the service to get the CSV data as a stream
            var stream = await _service.ExportEntriesToCsvAsync();

            // Return the stream as a file with the appropriate content type and filename
            return File(stream, "text/csv", "ContainerEntries.csv");
        }
    }
}
