namespace Board.Api.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = String.Empty;
        public string Author { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
