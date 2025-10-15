namespace BooksAPIDapper.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int? YearPublished { get; set; }
        public decimal? Price { get; set; }
    }
}
