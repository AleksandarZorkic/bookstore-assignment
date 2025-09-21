using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publishers;

        public PublishersController(IPublisherRepository publishers)
        {
            _publishers = publishers;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _publishers.GetAllAsync());

        // GET api/publishers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var items = await _publishers.GetByIdAsync(id);
            return items is null ? NotFound() : Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Publisher dto)
        {
            var created = await _publishers.AddAsync(dto);
            await _publishers.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
        }

        // PUT api/publishers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Publisher dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            if (!await _publishers.ExistsAsync(id)) return NotFound();

            await _publishers.UpdateAsync(dto);
            await _publishers.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _publishers.DeleteAsync(id);
                await _publishers.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict("Izdavac ne moze da ga obrise jer postoje knjige koje ga referenciraju.");
            }
        }
    }
}
