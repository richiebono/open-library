namespace LaunchQ.TakeHomeProject.Domain.Models
{
    /// <summary>
    /// Domain entity representing an Author
    /// </summary>
    public class Author
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Personal_name { get; set; }
        public string? Birth_date { get; set; }
        public string? Death_date { get; set; }
        public string? Bio { get; set; }
        public List<string> Alternate_names { get; set; } = new();
        public string? Title { get; set; }
        public List<string> Works { get; set; } = new();
        public List<BookSummary> Books { get; set; } = new();
        public string? OfficialSite { get; set; }
        public string? Wikipedia { get; set; }
        public Dictionary<string, string> Photos { get; set; } = new();
    }
}
