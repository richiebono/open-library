using System.Text.Json.Serialization;

namespace LaunchQ.TakeHomeProject.Application.DTOs
{
    /// <summary>
    /// DTO for pagination links from OpenLibrary API
    /// </summary>
    public class PaginationLinksDto
    {
        /// <summary>
        /// Link to the next page of results
        /// </summary>
        [JsonPropertyName("next")]
        public string? Next { get; set; }
        
        /// <summary>
        /// Link to the previous page of results
        /// </summary>
        [JsonPropertyName("prev")]
        public string? Previous { get; set; }
    }
}
