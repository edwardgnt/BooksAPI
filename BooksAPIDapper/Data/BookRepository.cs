using BooksAPIDapper.DTOs;
using BooksAPIDapper.Interfaces;
using BooksAPIDapper.Models;
using Dapper;
using System.Data;

namespace BooksAPIDapper.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;

        public BookRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<PagedResult<Book>> GetFilteredPagedAsync(BookFilterDto filter)
        {
            var baseSql = @"
                FROM dbo.Books
                WHERE IsArchived = 0
            ";

            var p = new DynamicParameters();

            if (filter.MinPrice.HasValue) 
            { 
                baseSql += " AND Price >= @MinPrice"; p.Add("MinPrice", filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue) 
            { 
                baseSql += " AND Price <= @MaxPrice"; p.Add("MaxPrice", filter.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                baseSql += " AND (Title LIKE @Search OR Author LIKE @Search)";
                p.Add("Search", $"%{filter.Search}%");
            }

            if (filter.Start.HasValue) { 
                baseSql += " AND CreatedAt >= @Start"; 
                p.Add("Start", filter.Start.Value);
            }

            if (filter.End.HasValue) { 
                baseSql += " AND CreatedAt <= @End"; 
                p.Add("End", filter.End.Value);
            }

            var orderBy = (filter.Sort?.ToLower()) switch
            {
                "price_desc" => " ORDER BY Price DESC",
                "price_asc" => " ORDER BY Price ASC",
                "title_desc" => " ORDER BY Title DESC",
                "title_asc" => " ORDER BY Title ASC",
                "year_desc" => " ORDER BY YearPublished DESC",
                "year_asc" => " ORDER BY YearPublished ASC",
                "created_desc" => " ORDER BY CreatedAt DESC",
                "created_asc" => " ORDER BY CreatedAt ASC",
                _ => " ORDER BY Id DESC"
            };

            // Total count
            var countSql = "SELECT COUNT(*) " + baseSql;
            var total = await _db.ExecuteScalarAsync<int>(countSql, p);

            // Paging
            var offset = (filter.Page - 1) * filter.PageSize;
            p.Add("Offset", offset);
            p.Add("PageSize", filter.PageSize);

            var dataSql = $@"
                SELECT Id, Title, Author, YearPublished, Price, CreatedAt, IsArchived
                {baseSql}
                {orderBy}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";

            var items = await _db.QueryAsync<Book>(dataSql, p);

            return new PagedResult<Book>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<IEnumerable<Book>> GetAllAsync() =>
            await _db.QueryAsync<Book>("SELECT * FROM Books WHERE IsArchived = 0");

        public async Task<Book?> GetByIdAsync(int id) =>
            await _db.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });

        public async Task<int> CreateAsync(Book book)
        {
            var sql = @"INSERT INTO Books (Title, Author, YearPublished, CreatedAt, IsArchived, Price)
                        VALUES (@Title, @Author, @YearPublished, @CreatedAt, @IsArchived, @Price);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.ExecuteScalarAsync<int>(sql, book);
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var sql = @"UPDATE Books
                        SET Title = @Title, Author = @Author, YearPublished = @YearPublished,
                            Price = @Price, IsArchived = @IsArchived
                        WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, book) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"UPDATE Books SET IsArchived = 1 WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id }) > 0;
        }
    }
}
