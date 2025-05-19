namespace LaunchQ.TakeHomeProject.Domain.Models
{
    /// <summary>
    /// Domain entity representing a Book
    /// </summary>
    public class Book
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string? Description { get; set; }
        public string? First_publish_date { get; set; }
        public string? Excerpt { get; set; }
        public Dictionary<string, string> Covers { get; set; } = new();
        public List<string> Subjects { get; set; } = new();
        public string? Isbn { get; set; }

        public string GetCoverImageUrl(string size = "M")
        {
            if (string.IsNullOrEmpty(Key))
                return string.Empty;

            var id = Key.Replace("/works/", "");
            return $"https://covers.openlibrary.org/b/id/{id}-{size}.jpg";
        }

        public string GetFormattedKey()
        {
            return Key.Replace("/works/", "");
        }
    }
}
