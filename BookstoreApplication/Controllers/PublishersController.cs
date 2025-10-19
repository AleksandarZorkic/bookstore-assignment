using Microsoft.AspNetCore.Mvc;
using BookstoreApplication.Models;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.Services;

namespace BookstoreApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _service;
    public PublishersController(IPublisherService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? sort)
        => Ok(await _service.GetAllSortedAsync(sort));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
        => (await _service.GetByIdAsync(id)) is { } a ? Ok(a) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Publisher dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Publisher dto)
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
    
}
