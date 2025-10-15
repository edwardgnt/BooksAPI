using BooksAPIDapper.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BooksAPIDapper.DTOs;
using BooksAPIDapper.Models;

namespace BooksAPIDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repo;
        public BooksController(IBookRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookFilterDto filter)
        {
            const int MaxRangeDays = 95;

            // Default End = now(UTC). Default Start = 1 day before End.
            filter.End ??= DateTime.UtcNow;
            filter.Start ??= filter.End.Value.AddDays(-1);

            if ((filter.End.Value - filter.Start.Value).TotalDays > MaxRangeDays)
            {
                return Problem(
                    title: "Date range too large",
                    detail: $"Max allowed range is {MaxRangeDays} days.",
                    statusCode: StatusCodes.Status400BadRequest
                    );
            }

            // Normal paging
            filter.Page = Math.Max(1, filter.Page);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 200);

            var page = await _repo.GetFilteredPagedAsync(filter);

            // Map to DTO
            var result = new
            {
                items = page.Items.Select(b => new BookReadDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    YearPublished = b.YearPublished,
                    Price = b.Price,
                    CreatedAt = b.CreatedAt
                }),
                page.TotalCount,
                page.Page,
                page.PageSize
            };

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b is null) return NotFound();

            var dto = new BookReadDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                YearPublished = b.YearPublished,
                Price = b.Price,
                CreatedAt = b.CreatedAt
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                YearPublished = dto.YearPublished ?? 0,
                Price = dto.Price ?? 0m
            };

            var newId = await _repo.CreateAsync(book);
            var created = await _repo.GetByIdAsync(newId);
            if (created is null) return Problem("Created but not found.");

            var read = new BookReadDto
            {
                Id = created.Id,
                Title = created.Title,
                Author = created.Author,
                YearPublished = created.YearPublished,
                Price = created.Price,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = read.Id }, read);
        }

        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null) return NotFound();

            existing.Title = dto.Title;
            existing.Author = dto.Author;
            existing.YearPublished = dto.YearPublished ?? existing.YearPublished;
            existing.Price = dto.Price ?? existing.Price;

            var ok = await _repo.UpdateAsync(existing);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAsync(id); 
            return !ok ? NotFound() : NoContent();
        }
    }
}
