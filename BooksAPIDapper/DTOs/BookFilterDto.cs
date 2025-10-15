namespace BooksAPIDapper.DTOs
{
    public class BookFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
