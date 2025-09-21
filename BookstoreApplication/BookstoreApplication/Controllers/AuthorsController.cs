using Microsoft.AspNetCore.Mvc;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorRepository _authors;
    public AuthorsController(IAuthorRepository authors) => _authors = authors;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _authors.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var a = await _authors.GetByIdAsync(id);
        return a is null ? NotFound() : Ok(a);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Author dto)
    {
        var created = await _authors.AddAsync(dto);
        await _authors.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Author dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        if (!await _authors.ExistsAsync(id)) return NotFound();

        await _authors.UpdateAsync(dto);
        await _authors.SaveChangesAsync();
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _authors.DeleteAsync(id);
        await _authors.SaveChangesAsync();
        return NoContent();
    }
}
