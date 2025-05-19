namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for link data from OpenLibrary API
    /// </summary>
    public class LinkDto
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public LinkTypeDto? Type { get; set; }
    }
}
