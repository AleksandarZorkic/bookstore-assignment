using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using BookstoreApplication.DTOs;
using BookstoreApplication.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using BookstoreApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/reviews")]
    [Authorize] 
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _svc;
        public ReviewsController(IReviewService svc) => _svc = svc;

        [HttpPost]
        public async Task<ActionResult> Create(int bookId, [FromBody] CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirst("sub")?.Value
                         ?? throw new UnauthorizedAccessException();

            var avg = await _svc.CreateAsync(userId, bookId, dto);
            return Created($"/api/books/{bookId}/reviews", new { averageRating = avg });
        }
    }
}

