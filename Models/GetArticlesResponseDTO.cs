namespace Models
{
    public class GetArticlesResponseDTO
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public GetArticlesResponseDataDTO[]? data { get; set; }
    }

    public class GetArticlesResponseDataDTO
    {
        public string? title { get; set; }
        public string? url { get; set; }
        public string? author { get; set; }
        public int? num_comments { get; set; }
        public object? story_id { get; set; }
        public string? story_title { get; set; }
        public string? story_url { get; set; }
        public int? parent_id { get; set; }
        public int created_at { get; set; }
    }
}