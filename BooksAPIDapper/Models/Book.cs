namespace BooksAPIDapper.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsArchived { get; set; }
        public decimal Price { get; set; }
    }
}
