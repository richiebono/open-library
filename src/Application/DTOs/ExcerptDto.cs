namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for excerpt from OpenLibrary API
    /// </summary>
    public class ExcerptDto
    {
        public string Text { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
