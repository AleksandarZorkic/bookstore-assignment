using BookstoreApplication.DTOs.Comics;
using BookstoreApplication.Models.Entities;
using BookstoreApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/comic-issues")]
    [ApiController]
    [Authorize(Roles = "Urednik")]
    public class ComicIssuesController : ControllerBase
    {
        private readonly IComicsService _svc;
        public ComicIssuesController(IComicsService svc) => _svc = svc;

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateComicIssueDto dto)
        {
            var id = await _svc.CreateIssueAsync(dto);
            return CreatedAtAction(nameof(GetOne), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ComicIssue>> GetOne([FromServices] AppDbContext db, int id)
        {
            var it = await db.ComicIssues.FindAsync(id);
            return it is null ? NotFound() : Ok(it);
        }
    }
}
