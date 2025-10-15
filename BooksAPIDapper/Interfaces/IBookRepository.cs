using BooksAPIDapper.DTOs;
using BooksAPIDapper.Models;

namespace BooksAPIDapper.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<int> CreateAsync(Book book);
        Task<bool> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);

        // Pagination and Filtering
        Task<PagedResult<Book>> GetFilteredPagedAsync(BookFilterDto filter);
    }
}
