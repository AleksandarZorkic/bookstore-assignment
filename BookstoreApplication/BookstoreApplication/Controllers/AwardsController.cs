using Microsoft.AspNetCore.Mvc;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AwardsController : ControllerBase
{
    private readonly IAwardRepository _awards;
    public AwardsController(IAwardRepository awards) => _awards = awards;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _awards.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var a = await _awards.GetByIdAsync(id);
        return a is null ? NotFound() : Ok(a);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Award dto)
    {
        var created = await _awards.AddAsync(dto);
        await _awards.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Award dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        if (!await _awards.ExistsAsync(id)) return NotFound();

        await _awards.UpdateAsync(dto);
        await _awards.SaveChangesAsync();
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _awards.DeleteAsync(id);
        await _awards.SaveChangesAsync();
        return NoContent();
    }
}
