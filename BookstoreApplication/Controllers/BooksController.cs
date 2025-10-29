using Microsoft.AspNetCore.Mvc;
using BookstoreApplication.Models;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.DTOs;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        public BooksController(IBookService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? sort = null)
            => Ok(await _service.GetAllSortedAsync(sort));

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] BookSearchRequestDto request)
            => Ok(await _service.SearchAsync(request));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book dto)
            => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
