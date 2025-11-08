using Microsoft.AspNetCore.Mvc;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services;

namespace BookstoreApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _service;
    public AuthorsController(IAuthorService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
        => (await _service.GetByIdAsync(id)) is { } a ? Ok(a) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Author dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Author dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok(dto);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
    [HttpGet("page")]
    public async Task<IActionResult> GetPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var page = await _service.GetPageAsync(pageNumber, pageSize);
        return Ok(page);
    }
}
