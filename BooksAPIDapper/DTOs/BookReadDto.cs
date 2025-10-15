namespace BooksAPIDapper.DTOs
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int YearPublished { get; set; }
        public decimal? Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
